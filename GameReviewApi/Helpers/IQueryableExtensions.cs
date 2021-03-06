﻿using GameReviewApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace GameReviewApi.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy, 
            Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (mappingDictionary == null)
            {
                throw new ArgumentNullException("mappingDictionary");
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var orderByAfterSplit = orderBy.Split(",");

            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                var trimmedOrderByClause = orderByClause.Trim();

                var orderDescending = trimmedOrderByClause.EndsWith(" desc");

                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ? 
                    trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }

                var propertyMappingValue = mappingDictionary[propertyName];

                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException("propertyMappingValue");
                }

                foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    if (propertyMappingValue.Revert)
                    {
                        orderDescending = !orderDescending;
                    }

                    source = source.OrderBy(destinationProperty + (orderDescending ? " decending" : " ascending"));
                }
            }
            return source;
        }

        public static IQueryable<object> ShapeData<TSource>(this IQueryable<TSource> source,
            string fields,
            Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (mappingDictionary == null)
            {
                throw new ArgumentNullException("mappingDictionary");
            }

            if (string.IsNullOrWhiteSpace(fields))
            {
                return (IQueryable<object>)source;
            }

            fields = fields.ToLower();

            var fieldsAfterSplit = fields.Split(',');

            var selectClause = "new (";

            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();

                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }

                var propertyMappingValue = mappingDictionary[propertyName];

                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException("propertyMappingValue");
                }

                foreach (var destinationProperty in propertyMappingValue.DestinationProperties)
                {
                    selectClause += $" {destinationProperty},";
                }
            }

            selectClause = selectClause.Substring(0, selectClause.Length - 1) + ")";
            return (IQueryable<object>)source.Select(selectClause);
        }
    }
}
