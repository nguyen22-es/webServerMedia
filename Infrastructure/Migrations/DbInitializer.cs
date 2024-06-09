using Common.Entities;
using Database.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Database.Migrations
{
    public class DbInitializer
    {
        public static void RecreateDatabase(VODContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        public static async void Initialize(VODContext context)
        {
            #region Lorem Ipsum - Dummy Data
            var description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
            #endregion

            #region Admin Credentials Properties
            /*
                The email address should be in the AspNetUsers table; if not, 
                then register a user with that email address or change the variable 
                value to an email address in the table. The user should be an 
                administrator; if not, open the AspNetUserRoles table and add 
                a record using the user id and 1 (or the id you gave the Admin
                role in the AspNetRoles table) in the RoleId column.
             */
            var email = "a@b.c";
            var adminRoleId = string.Empty;
            var userId = string.Empty;
            #endregion

            // Fetch the User Data
            if (context.Users.Any(r => r.Email.Equals(email)))
                userId = context.Users.First(r => r.Email.Equals(email)).Id;

            if (!userId.Equals(string.Empty))
            {
                #region Add Instructors if they don't already exist
                if (!context.topicTypes.Any())
                {
                    var topicTypes = new List<TopicType>
                    {
                        new TopicType {
                            Name = "John Doe",
                            Description = description.Substring(20, 50),
                            Thumbnail = "/images/Ice-Age-Scrat-icon.png"
                        },
                        new TopicType {
                            Name = "Jane Doe",
                            Description = description.Substring(30, 40),
                            Thumbnail = "/images/Ice-Age-Scrat-icon.png"
                        }
                    };
                 await   context.topicTypes.AddRangeAsync(topicTypes);
                 await   context.SaveChangesAsync();
                }
                #endregion

                #region Add Courses if they don't already exist
                if (!context.topics.Any())
                {
                    var TopicTypeId1 = context.topicTypes.First().Id;
                    var TopicTypeId2 = int.MinValue;
                    var topicType = context.topicTypes.Skip(1).FirstOrDefault();
                    if (topicType != null) TopicTypeId2 = topicType.Id;
                    else TopicTypeId2 = TopicTypeId1;

                    var topics = new List<Topic>
                    {
                        new Topic {
                            TopicTypeId = TopicTypeId1,
                            Title = "Course 1",
                            Description = description,
                            ImageUrl = "/images/course1.jpg",
                            MarqueeImageUrl = "/images/laptop.jpg"
                        },
                        new Topic {
                            TopicTypeId = TopicTypeId2,
                            Title = "Course 2",
                            Description = description,
                            ImageUrl = "/images/course2.jpg",
                            MarqueeImageUrl = "/images/laptop.jpg"
                        },
                        new Topic {
                            TopicTypeId = TopicTypeId1,
                            Title = "Course 3",
                            Description = description,
                            ImageUrl = "/images/course3.jpg",
                            MarqueeImageUrl = "/images/laptop.jpg"
                        }
                    };
                 await   context.topics.AddRangeAsync(topics);
                    await context.SaveChangesAsync();
                }
                #endregion

                #region Fetch Course ids if any courses exists
                var topicId1 = int.MinValue;
                var topicId2 = int.MinValue;
                var topicId3 = int.MinValue;
                if (context.topics.Any())
                {
                    topicId1 = context.topics.First().Id;

                    var course = context.topics.Skip(1).FirstOrDefault();
                    if (course != null) topicId2 = course.Id;

                    course = context.topics.Skip(2).FirstOrDefault();
                    if (course != null) topicId3 = course.Id;
                }
                #endregion

                #region Add UserCourses connections if they don't already exist
                if (!context.userTopics.Any())
                {
                    if (!topicId1.Equals(int.MinValue))
                      await  context.userTopics.AddAsync(new UserTopic { UserId = userId, TopicId = topicId1 });

                    if (!topicId2.Equals(int.MinValue))
                        await context.userTopics.AddAsync(new UserTopic { UserId = userId, TopicId = topicId2 });

                    if (!topicId3.Equals(int.MinValue))
                        await context.userTopics.AddAsync(new UserTopic { UserId = userId, TopicId = topicId3 });

                  await  context.SaveChangesAsync();
                }
                #endregion

                #region Add Modules if they don't already exist
                if (!context.Modules.Any())
                {
                    var modules = new List<Module>
                    {
                        new Module { Topic = context.Find<Topic>(topicId1), Title = "Modeule 1" },
                        new Module { Topic = context.Find<Topic>(topicId2), Title = "Modeule 2" },
                        new Module { Topic = context.Find<Topic>(topicId3), Title = "Modeule 3" }
                    };
                    await context.Modules.AddRangeAsync(modules);
                    await context.SaveChangesAsync();
                }
                #endregion

                #region Fetch Module ids if any modules exist
                var moduleId1 = int.MinValue;
                var moduleId2 = int.MinValue;
                var moduleId3 = int.MinValue;
                if (context.Modules.Any())
                {
                    moduleId1 = context.Modules.First().Id;

                    var module = context.Modules.Skip(1).FirstOrDefault();
                    if (module != null) moduleId2 = module.Id;
                    else moduleId2 = moduleId1;

                    module = context.Modules.Skip(2).FirstOrDefault();
                    if (module != null) moduleId3 = module.Id;
                    else moduleId3 = moduleId1;
                }
                #endregion

                #region Add Videos if they don't already exist
                if (!context.medias.Any())
                {
                    var medias = new List<Media>
                    {
                        new Media { ModuleId = moduleId1, TopicId = topicId1,
                            Title = "Video 1 Title",
                            Description = description.Substring(1, 35),
                            Duration = 50, Thumbnail = "/images/video1.jpg",
                            Url = "https://www.youtube.com/embed/BJFyzpBcaCY"
                        },
                        new Media { ModuleId = moduleId1, TopicId = topicId2,
                            Title = "Video 2 Title",
                            Description = description.Substring(5, 35),
                            Duration = 45, Thumbnail = "/images/video2.jpg",
                            Url = "https://www.youtube.com/embed/BJFyzpBcaCY"
                        },
                          new Media { ModuleId = moduleId1, TopicId = topicId3,
                            Title = "Video 3 Title",
                            Description = description.Substring(5, 35),
                            Duration = 45, Thumbnail = "/images/video2.jpg",
                            Url = "https://www.youtube.com/embed/BJFyzpBcaCY"
                        },

                    };
                   await context.medias.AddRangeAsync(medias);
                  await context.SaveChangesAsync();
                }
                #endregion

                #region Add Downloads if they don't already exist
                if (!context.Downloads.Any())
                {
                    var downloads = new List<Download>
                    {
                        new Download{ModuleId = moduleId1,  TopicId = topicId1,
                            Title = "ADO.NET 1 (PDF)", Url = "https://some-url" },

                        new Download{ModuleId = moduleId1, TopicId = topicId2,
                            Title = "ADO.NET 2 (PDF)", Url = "https://some-url" },

                        new Download{ModuleId = moduleId3, TopicId = topicId3,
                            Title = "ADO.NET 1 (PDF)", Url = "https://some-url" }
                    };

                  await  context.Downloads.AddRangeAsync(downloads);
                  await  context.SaveChangesAsync();
                }
                #endregion
            }
        }
    }
}
