﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.Models;
using WVUPSM.Models.Entities;
using System.Linq;

namespace WVUPSM.DAL.Initiliazers
{
    /// <summary>
    ///     Sample Data for WVUPSM
    /// </summary>
    public static class SampleData
    {
        /// <summary>
        ///     Gets all users with their own posts
        /// </summary>
        /// <returns>Collection of Users</returns>
        public static IEnumerable<User> GetUsers()
        {
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            List<User> userList = new List<User>();
            List<Post> postList = new List<Post>()
            
            {
                PostGenerator(5),
                PostGenerator(20),
                PostGenerator(50),
                PostGenerator(25),
                PostGenerator(40),
                PostGenerator(69),
                PostGenerator(75),
                PostGenerator(75),
                PostGenerator(75),
                PostGenerator(15),
                PostGenerator(100),
                PostGenerator(19),
                PostGenerator(15),
                PostGenerator(45),
                PostGenerator(75),
                PostGenerator(69)
            };

            //NOTE: the convertAll is for DEEP COPIES, shallow will not work

            User levi = CreateNewUser("leviB");
            levi.Posts = postList.ConvertAll(x =>
            new Post()
            {
                Text = x.Text
            });
                        
            User sean = CreateNewUser("seanR");
            sean.Posts = postList.ConvertAll(x =>
            new Post()
            {
                Text = x.Text
            });

            User sam = CreateNewUser("samB");
            sam.Posts = postList.ConvertAll(x =>
            new Post()
            {
                Text = x.Text
            });

            
            User ben = CreateNewUser("ben10");
            ben.Posts = postList.ConvertAll(x =>
            new Post()
            {
                Text = x.Text
            });

            User kingOfGames = CreateNewUser("king0fGames");
            kingOfGames.Posts = postList.ConvertAll(x =>
            new Post()
            {
                Text = x.Text
            });

            User l = CreateNewUser("l");
            l.Posts = postList.ConvertAll(x =>
            new Post()
            {
                Text = x.Text
            });

            User kanye = CreateNewUser("kanye", "roblox@develop.com");
            kanye.Posts = postList.ConvertAll(x =>
            new Post()
            {
                Text = x.Text
            });

            User jojo = CreateNewUser("jojo");
            jojo.Posts = postList.ConvertAll(x =>
            new Post()
            {
                Text = x.Text
            });

            User zz = CreateNewUser("zz", "top@develop.com");
            zz.Posts = postList.ConvertAll(x =>
            new Post()
            {
                Text = x.Text
            });

            User scooby = CreateNewUser("scooby", "doo@develop.com");
            scooby.Posts = postList.ConvertAll(x =>
            new Post()
            {
                Text = x.Text
            });

            User digital = CreateNewUser("digital","dummy@develop.com");
            digital.Posts = postList.ConvertAll(x =>
            new Post()
            {
                Text = x.Text
            });

            User admin = CreateNewUser("Admin");

            userList.Add(levi);
            userList.Add(sean);
            userList.Add(sam);
            userList.Add(ben);
            userList.Add(kingOfGames);
            userList.Add(l);
            userList.Add(kanye);
            userList.Add(jojo);
            userList.Add(zz);
            userList.Add(scooby);
            userList.Add(digital);
            userList.Add(admin);

            return SetPasswords(userList);
        }

        /// <summary>
        ///     Returns a list of Users that will be used in Production
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<User> GetProdUsers()
        {
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            List<User> userList = new List<User>();
            userList.Add(CreateNewUser("tester","test@Develop.com"));
            userList.Add(CreateNewUser("bravo", "bravo@Develop.com"));
            return SetPasswords(userList);
        }

        /// <summary>
        ///     Creates a new user, provide a userName and email will be userName@Develop.com or provided a email as well
        /// </summary>
        /// <param name="userName">doesn't need to be unique</param>
        /// <param name="email">has to be unique string with EX: @{Develop}.{com}</param>
        /// <returns>A new User object with necessary properties for Database set</returns>
        private static User CreateNewUser(string userName, string email = null)
        {
            return new User()
            {
                Email = email != null ? email : userName + "@develop.com",
                NormalizedEmail = email != null ? email.ToUpper(): userName.ToUpper() + "@DEVELOP.COM",
                UserName = userName,
                EmailConfirmed = true,
                NormalizedUserName = userName.ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
            };
        }

        /// <summary>
        ///     Generates a post with the amount of words passed in
        /// </summary>
        /// <remarks>
        ///     Posts words are randomly selected from a list of predefined words
        /// </remarks>
        /// <param name="words">amount of words post should have</param>
        /// <returns>A new post with the amount of words provided</returns>
        private static Post PostGenerator(int words)
        {
            Post post = new Post();
            string[] possibleWords = { "Loren", "Ispun", "Katchum", "WVUP", "Roblox", "Salt"};
            Random randm = new Random();
            for(int i = 0; i < words; i++)
            {
                post.Text += possibleWords[randm.Next(0, possibleWords.Length)] + " ";
            }

            return post;
        }

        /// <summary>
        ///     Gets a Collection of Follow records for the users provided
        /// </summary>
        /// <remarks>
        ///     users are randomly selected to follow other users,
        ///     each user will be following at least 5 people to the max of the count of users provided
        /// </remarks>
        /// <param name="users">users</param>
        /// <returns>Follow collection that has been randomly selected</returns>
        public static IEnumerable<Follow> GetFollowing(List<User> users)
        {
            //Seed random so follow list is consistent
            int seed = 420;
            Random random = new Random(seed);
            List<Follow> follows = new List<Follow>();

            //Have each user create a follow list
            foreach(User user in users)
            {
                //random for loop to go between 0 and users.count -1 
                //So each follow list is different
                int minFollowers = 5;
                int maxFollowers = random.Next(minFollowers, users.Count);
                for(int i = 0; i < maxFollowers; i++)
                {
                    User toFollow = users[random.Next(0, users.Count)];
                    if (toFollow.Id != user.Id)
                    {
                        Follow follow = CreateFollow(user, toFollow);
                        //Ensures no repeat follows
                        if (!follows.Exists(x => x.UserId == follow.UserId && x.FollowId == follow.FollowId))
                        {
                            follows.Add(follow);
                        }
                    }
                }
            }

            return follows;
        }
            
        /// <summary>
        ///     Creates a new user record
        /// </summary>
        /// <param name="user">User who will follow someone</param>
        /// <param name="goingToBeFollowed">The person who will be followed</param>
        /// <returns>Returns a new Follow object</returns>
        private static Follow CreateFollow(User user, User goingToBeFollowed)
            => new Follow()
            {
                UserId = user.Id,
                FollowId = goingToBeFollowed.Id
            };
        

        /// <summary>
        ///     Sets a list of users password to Develop@90
        /// </summary>
        /// <param name="users">users</param>
        /// <returns>Return the users with their password hash set</returns>
        private static IEnumerable<User> SetPasswords(List<User> users)
        {
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

            foreach (var employee in users)
            {
                employee.PasswordHash = passwordHasher.HashPassword(employee, "Develop@90");
            }

            return users;
        }

        /// <summary>
        ///     Returns a list of Groups
        /// </summary>
        /// <param name="users">users</param>
        /// <returns>Return the list of Groups</returns>
        public static IEnumerable<Group> GetGroups(List<User> users)
        {
            List<Group> groups = new List<Group>();
            Group Stem420 = CreateNewGroup("STEM420", users.ElementAtOrDefault(0));
            Group Stem300 = CreateNewGroup("STEM300", users.ElementAtOrDefault(1));
            Group DataStructures = CreateNewGroup("Data Structures", users.ElementAtOrDefault(2));

            groups.Add(Stem420);
            groups.Add(Stem300);
            groups.Add(DataStructures);

            return groups;
        }

        /// <summary>
        ///     Creates a new Group based on group name and user passed in
        /// </summary>
        /// <param name="name">name of group</param>
        /// <param name="user">group owner</param>
        /// <returns>Return the list of Groups</returns>
        public static Group CreateNewGroup(string name, User user)
        {
            return new Group()
            {
                Name = name,
                OwnerId = user.Id
            };
        }


        /// <summary>
        ///    Returns a list of members of each group
        /// </summary>
        /// <param name="users">Users to be added to passed in groups</param>
        /// <param name="groups">groups to be added to</param>
        /// <returns>Return the list of members for each Group</returns>
        public static IEnumerable<UserGroup> GetUserGroups(List<User> users, List<Group> groups)
        {
            List<UserGroup> userGroups = new List<UserGroup>();
            groups.OrderBy(group => group.Id);

            List<User> stem420Members = users.GetRange(3, 2);
            List<User> stem300Members = users.GetRange(6, 2);
            List<User> dataStructuresMembers = users.GetRange(7, 2);

            
            foreach(User user in stem420Members)
            {
                userGroups.Add(new UserGroup()
                {
                    GroupId =   groups.ElementAt(0).Id,
                    UserId = user.Id
                });
            }

            foreach (User user in stem300Members)
            {
                userGroups.Add(new UserGroup()
                {
                    GroupId = groups.ElementAt(1).Id,
                    UserId = user.Id
                });
            }

            foreach (User user in dataStructuresMembers)
            {
                userGroups.Add(new UserGroup()
                {
                    GroupId = groups.ElementAt(2).Id,
                    UserId = user.Id
                });
            }
            return userGroups;
        }

        /// <summary>
        ///     Creates and retruns posts for passed in groups
        /// </summary>
        /// <param name="users">Users making group posts</param>
        /// <param name="groups">Groups in which to make Posts</param>
        /// <returns>Return the list of GroupPosts</returns>
        public static IEnumerable<Post> GetGroupPosts(List<User> users, List<Group> groups)
        {
            List<Post> posts = new List<Post>();
            users.OrderBy(user => user.Id);

            posts.Add(new Post()
            {
                UserId = users.ElementAt(1).Id,
                GroupId = groups.ElementAt(0).Id,
                Text = "AsdfAsdf"
            });

            posts.Add(new Post()
            {
                UserId = users.ElementAt(2).Id,
                GroupId = groups.ElementAt(1).Id,
                Text = "AsdfAsdf"
            });

            posts.Add(new Post()
            {
                UserId = users.ElementAt(3).Id,
                GroupId = groups.ElementAt(2).Id,
                Text = "AsdfAsdf"
            });

            return posts;
        }


        /// <summary>
        ///     Creates a conversation between two users 
        /// </summary>
        /// <param name="sender">User who will follow someone</param>
        /// <param name="receiver">The person who will be followed</param>
        /// <returns>A list of messages where the senderId and receiverId alternate between the two passed in users</returns>
        public static IEnumerable<Message> CreateConversations(User sender, User receiver)
        {
            List<Message> messages = new List<Message>();

            for(int i = 0; i < 50; i++)
            {
                if (i % 2 == 0)
                {
                    messages.Add(new Message()
                    {
                        SenderId = sender.Id,
                        ReceiverId = receiver.Id,
                        Text = MessageText(i)
                    });
                }
                else
                {
                    messages.Add(new Message()
                    {
                        SenderId = receiver.Id,
                        ReceiverId = sender.Id,
                        Text = MessageText(i)
                    });
                }
            }

            return messages;
        }

        /// <summary>
        ///    Creates dummy text for messages between users
        /// </summary>
        /// <param name="seed">seed number for random count of phrases in message</param>
        /// <returns>A collection of a random number of phrases</returns>
        public static string MessageText(int seed)
        {
            Random random = new Random(seed);
            int randomNumber = random.Next(0, 11);
            string[] possiblePhrases = { "hi every1 im new!!!!!!!" , "*holds up spork* my name is katy but u can call me t3h PeNgU1N oF d00m!!!!!!!!",
                                        "lol…as u can see im very random!!!! thats why i came here, 2 meet random ppl like me ^_^",
                                        " im 13 years old (im mature 4 my age tho!!)", "i like 2 watch invader zim w/ my girlfreind (im bi if u dont like it deal w/it)",
                                        "its our favorite tv show!!! bcuz its SOOOO random!!!!", "shes random 2 of course but i want 2 meet more random ppl =) ",
                                        "like they say the more the merrier!!!! lol…", "neways i hope 2 make alot of freinds here so give me lots of commentses!!!!",
                                        "DOOOOOMMMM!!!!!!!!!!!!!!!! <--- me bein random again ^_^ hehe…toodles!!!!!","love and waffles" };
            return possiblePhrases[randomNumber];
        }

        /// <summary>
        ///     Creates a new Comment on a Post
        /// </summary>
        /// <param name="posts">List of Posts to be commented on</param>
        /// <param name="users">List of Users to create comments</param>
        /// <returns>Return the list of Comments</returns>
        public static IEnumerable<Comment> CreateComments(List<Post> posts, List<User> users)
        {
            List<Comment> comments = new List<Comment>();
            Random random = new Random();

            foreach (Post post in posts)
            {
                User user = users[random.Next(0, users.Count)];
                Post forCommentText = PostGenerator(random.Next(0, users.Count));

                comments.Add(new Comment()
                {
                    PostId = post.Id,
                    UserId = user.Id,
                    Text = forCommentText.Text
            });
            }
            return comments;
        }

        /// <summary>
        ///     Creates Tags siginifying personal Interest for passed in Users
        /// </summary>
        /// <param name="users">List of users to create Tags</param>
        /// <param name="interests">A string, to be delimited by spaces, with each section being a new Tag</param>
        /// <returns>Return the list of Tags</returns>
        //public static IEnumerable<Tag> CreateTags(List<User> users, List<string> interests)
        //{

        //}

        /// <summary>
        ///     Gets UserRoles
        /// </summary>
        /// <returns>Return the list of UserRoles</returns>
        public static IEnumerable<IdentityRole> GetRoles => new List<IdentityRole>
        {
            new IdentityRole()
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new IdentityRole()
            {
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
        };

        /// <summary>
        ///     Creates a new Group based on group name and user passed in
        /// </summary>
        /// <param name="users">A list of Users</param>
        /// <param name="roles">A list of UserRoles</param>
        /// <returns>Return the list of Users with their role</returns>
        public static IEnumerable<IdentityUserRole<string>> GetUserWithRole(List<User> users, List<IdentityRole> roles) 
            => new List<IdentityUserRole<string>>
        {
            new IdentityUserRole<string>()
            {
                RoleId = roles.Where(x => x.Name == "User").FirstOrDefault().Id,
                UserId = users.Where(x => x.UserName == "samB").FirstOrDefault().Id
            },
            new IdentityUserRole<string>()
            {
                RoleId = roles.Where(x => x.Name == "User").FirstOrDefault().Id,
                UserId = users.Where(x => x.UserName == "leviB").FirstOrDefault().Id
            },
            new IdentityUserRole<string>()
            {
                RoleId = roles.Where(x => x.Name == "User").FirstOrDefault().Id,
                UserId = users.Where(x => x.UserName == "seanR").FirstOrDefault().Id
            },
            new IdentityUserRole<string>()
            {
                RoleId = roles.Where(x => x.Name == "User").FirstOrDefault().Id,
                UserId = users.Where(x => x.UserName == "ben10").FirstOrDefault().Id
            },
            new IdentityUserRole<string>()
            {
                RoleId = roles.Where(x => x.Name == "User").FirstOrDefault().Id,
                UserId = users.Where(x => x.UserName == "digital").FirstOrDefault().Id
            },
            new IdentityUserRole<string>()
            {
                RoleId = roles.Where(x => x.Name == "User").FirstOrDefault().Id,
                UserId = users.Where(x => x.UserName == "scooby").FirstOrDefault().Id
            },
            new IdentityUserRole<string>()
            {
                RoleId = roles.Where(x => x.Name == "User").FirstOrDefault().Id,
                UserId = users.Where(x => x.UserName == "zz").FirstOrDefault().Id
            },
            new IdentityUserRole<string>()
            {
                RoleId = roles.Where(x => x.Name == "User").FirstOrDefault().Id,
                UserId = users.Where(x => x.UserName == "jojo").FirstOrDefault().Id
            },
            new IdentityUserRole<string>()
            {
                RoleId = roles.Where(x => x.Name == "User").FirstOrDefault().Id,
                UserId = users.Where(x => x.UserName == "kanye").FirstOrDefault().Id
            },
            new IdentityUserRole<string>()
            {
                RoleId = roles.Where(x => x.Name == "User").FirstOrDefault().Id,
                UserId = users.Where(x => x.UserName == "l").FirstOrDefault().Id
            },
            new IdentityUserRole<string>()
            {
                RoleId = roles.Where(x => x.Name == "User").FirstOrDefault().Id,
                UserId = users.Where(x => x.UserName == "king0fGames").FirstOrDefault().Id
            },
            new IdentityUserRole<string>()
            {
                RoleId = roles.Where(x => x.Name == "Admin").FirstOrDefault().Id,
                UserId = users.Where(x => x.UserName == "Admin").FirstOrDefault().Id
            },
        };
    }
}
