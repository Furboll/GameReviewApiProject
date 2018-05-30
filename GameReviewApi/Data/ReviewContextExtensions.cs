using GameReviewApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameReviewApi.Data
{
    public static class ReviewContextExtensions
    {
        public static void EnsureSeedDataForContext(this ReviewContext context)
        {
            context.Reviews.RemoveRange(context.Reviews);
            context.SaveChanges();

            var reviews = new List<Review>()
            {
                new Review()
                {
                    Id = 1,
                    ReviewTitle = "Super Mario Bros X Time",
                    Author = "Furball",
                    VideoUrl = "thisvideolinkdoesnotexistyet",
                    Introduction = "Come on, it will be fun!",
                    Body = "There is alot of text that could go here but this is just for testing!",
                    Conclusion = "How does one know that this is the end? :thinking:",
                    DatePosted = new DateTime(2018, 1, 1),
                    Game = new Game()
                    {
                        Id = 1,
                        Title = "Super Mario Bros",
                        Genre = "Platformer",
                        Publisher = "Nintendo",
                        Developer = "Nintendo",
                        ReleaseDate = new DateTime(1985,9,13)
                    },
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            CommentId = 1,
                            Author = "Schmuck",
                            CommentContent = "I are Schmuck, this review Schmuck",
                            DatePosted = new DateTime(2018, 1, 2),
                        }
                    }
                },
                new Review()
                {
                    Id = 2,
                    ReviewTitle = "The Legend of Zelda X Time",
                    Author = "Furball",
                    VideoUrl = "thisvideolinkdoesnotexistyet",
                    Introduction = "Come on, it will be fun!",
                    Body = "There is alot of text that could go here but this is just for testing!",
                    Conclusion = "How does one know that this is the end? :thinking:",
                    DatePosted = new DateTime(2018, 2, 1),
                    Game = new Game()
                    {
                        Id = 2,
                        Title = "The Legend of Zelda",
                        Genre = "Action-adventure",
                        Publisher = "Nintendo",
                        Developer = "Nintendo Research & Development 4",
                        ReleaseDate = new DateTime(1986,2,21)
                    },
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            CommentId = 2,
                            Author = "Schmuckie",
                            CommentContent = "I are Schmuckie, this review Schmuckie",
                            DatePosted = new DateTime(2018, 2, 2),
                        }
                    }
                },
                new Review()
                {
                    Id = 3,
                    ReviewTitle = "Mega Man X Time",
                    Author = "Furball",
                    VideoUrl = "thisvideolinkdoesnotexistyet",
                    Introduction = "Come on, it will be fun!",
                    Body = "There is alot of text that could go here but this is just for testing!",
                    Conclusion = "How does one know that this is the end? :thinking:",
                    DatePosted = new DateTime(2018, 3, 1),
                    Game = new Game()
                    {
                        Id = 3,
                        Title = "Mega Man",
                        Genre = "Action Platform",
                        Publisher = "Capcom",
                        Developer = "Capcom",
                        ReleaseDate = new DateTime(1987,12,17)
                    },
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            CommentId = 3,
                            Author = "Schmucke",
                            CommentContent = "I are Schmucke, this review Schmucke",
                            DatePosted = new DateTime(2018, 3, 2),
                        }
                    }
                },
                new Review()
                {
                    Id = 4,
                    ReviewTitle = "Battletoads X Time",
                    Author = "Furball",
                    VideoUrl = "thisvideolinkdoesnotexistyet",
                    Introduction = "Come on, it will be fun!",
                    Body = "There is alot of text that could go here but this is just for testing!",
                    Conclusion = "How does one know that this is the end? :thinking:",
                    DatePosted = new DateTime(2018, 4, 1),
                    Game = new Game()
                    {
                        Id = 4,
                        Title = "Battletoads",
                        Genre = "Beat em up Platformer",
                        Publisher = "Tradewest",
                        Developer = "Rare",
                        ReleaseDate = new DateTime(1991,6,1)
                    },
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            CommentId = 4,
                            Author = "Schmucko",
                            CommentContent = "I are Schmucko, this review Schmucko",
                            DatePosted = new DateTime(2018, 4, 2),
                        }
                    }
                },
                new Review()
                {
                    Id = 5,
                    ReviewTitle = "Seiken Densetsu 3 X Time",
                    Author = "Furball",
                    VideoUrl = "thisvideolinkdoesnotexistyet",
                    Introduction = "Come on, it will be fun!",
                    Body = "There is alot of text that could go here but this is just for testing!",
                    Conclusion = "How does one know that this is the end? :thinking:",
                    DatePosted = new DateTime(2018, 5, 1),
                    Game = new Game()
                    {
                        Id = 5,
                        Title = "Seiken Densetsu 3",
                        Genre = "Action RPG",
                        Publisher = "Squaresoft",
                        Developer = "Squaresoft",
                        ReleaseDate = new DateTime(1995,9,30)
                    },
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            CommentId = 6,
                            Author = "Schmucka",
                            CommentContent = "I are Schmucka, this review Schmucka",
                            DatePosted = new DateTime(2018, 5, 2),
                        }
                    }
                },
                new Review()
                {
                    Id = 6,
                    ReviewTitle = "Chrono Trigger X Time",
                    Author = "Furball",
                    VideoUrl = "thisvideolinkdoesnotexistyet",
                    Introduction = "Come on, it will be fun!",
                    Body = "There is alot of text that could go here but this is just for testing!",
                    Conclusion = "How does one know that this is the end? :thinking:",
                    DatePosted = new DateTime(2018, 6, 1),
                    Game = new Game()
                    {
                        Id = 6,
                        Title = "Chrono Trigger",
                        Genre = "RPG",
                        Publisher = "Squaresoft",
                        Developer = "Squaresoft",
                        ReleaseDate = new DateTime(1985,9,13)
                    },
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            CommentId = 6,
                            Author = "Schmucku",
                            CommentContent = "I are Schmucku, this review Schmucku",
                            DatePosted = new DateTime(2018, 6, 2),
                        }
                    }
                },
                new Review()
                {
                    Id = 7,
                    ReviewTitle = "TMNT: Turtles in Time X Time",
                    Author = "Furball",
                    VideoUrl = "thisvideolinkdoesnotexistyet",
                    Introduction = "Come on, it will be fun!",
                    Body = "There is alot of text that could go here but this is just for testing!",
                    Conclusion = "How does one know that this is the end? :thinking:",
                    DatePosted = new DateTime(2018, 7, 1),
                    Game = new Game()
                    {
                        Id = 7,
                        Title = "Teenage Mutant Ninja Turtles: Turtles in Time",
                        Genre = "Side-scrolling Beat em up",
                        Publisher = "Konami",
                        Developer = "Konami",
                        ReleaseDate = new DateTime(1992,6,24)
                    },
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            CommentId = 7,
                            Author = "Schmucky",
                            CommentContent = "I are Schmucky, this review Schmucky",
                            DatePosted = new DateTime(2018, 7, 2),
                        }
                    }
                },
                new Review()
                {
                    Id = 8,
                    ReviewTitle = "Super Street Fighter 2: The New Challengers X Time",
                    Author = "Furball",
                    VideoUrl = "thisvideolinkdoesnotexistyet",
                    Introduction = "Hadouken!",
                    Body = "Shoryuken!",
                    Conclusion = "Tatsumaki Senpukyaku!",
                    DatePosted = new DateTime(2018, 8, 1),
                    Game = new Game()
                    {
                        Id = 8,
                        Title = "Super Street Fighter 2: The New Challengers",
                        Genre = "Fighting",
                        Publisher = "Capcom",
                        Developer = "Capcom",
                        ReleaseDate = new DateTime(1993,9,10)
                    },
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            CommentId = 8,
                            Author = "Schmuckou",
                            CommentContent = "I are Schmuckou, this review Schmuckou",
                            DatePosted = new DateTime(2018, 8, 2),
                        }
                    }
                },
                new Review()
                {
                    Id = 9,
                    ReviewTitle = "The Legend of Zelda: Ocarina of Time X Time",
                    Author = "Furball",
                    VideoUrl = "thisvideolinkdoesnotexistyet",
                    Introduction = "Come on, it will be fun!",
                    Body = "There is alot of text that could go here but this is just for testing!",
                    Conclusion = "How does one know that this is the end? :thinking:",
                    DatePosted = new DateTime(2018, 9, 1),
                    Game = new Game()
                    {
                        Id = 9,
                        Title = "The Legend of Zelda: Ocarina of Time",
                        Genre = "Action-adventure",
                        Publisher = "Nintendo",
                        Developer = "Nintendo EAD",
                        ReleaseDate = new DateTime(1998,11,21)
                    },
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            CommentId = 9,
                            Author = "Schmuckoo",
                            CommentContent = "I are Schmuckoo, this review Schmuckoo",
                            DatePosted = new DateTime(2018, 9, 2),
                        }
                    }
                },
                new Review()
                {
                    Id = 10,
                    ReviewTitle = "Super Mario 64 X Time",
                    Author = "Furball",
                    VideoUrl = "thisvideolinkdoesnotexistyet",
                    Introduction = "Come on, it will be fun!",
                    Body = "There is alot of text that could go here but this is just for testing!",
                    Conclusion = "How does one know that this is the end? :thinking:",
                    DatePosted = new DateTime(2018, 10, 1),
                    Game = new Game()
                    {
                        Id = 10,
                        Title = "Super Mario 64",
                        Genre = "Action-adventure",
                        Publisher = "Nintendo",
                        Developer = "Nintendo EAD",
                        ReleaseDate = new DateTime(1996,6,23)
                    },
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            CommentId = 10,
                            Author = "Schmuckae",
                            CommentContent = "I are Schmuckae, this review Schmuckae",
                            DatePosted = new DateTime(2018, 10, 2),
                        }
                    }
                }
            };

            context.Reviews.AddRange(reviews);
            context.SaveChanges();
        }
    }
}
