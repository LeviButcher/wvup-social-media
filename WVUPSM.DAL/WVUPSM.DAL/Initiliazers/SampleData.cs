﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.Models;
using WVUPSM.Models.Entities;
using System.Linq;

namespace WVUPSM.DAL.Initiliazers
{
    public static class SampleData
    {
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

            return SetPasswords(userList);
        }

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
            

        private static Follow CreateFollow(User user, User goingToBeFollowed)
            => new Follow()
            {
                UserId = user.Id,
                FollowId = goingToBeFollowed.Id
            };
        

        private static IEnumerable<User> SetPasswords(List<User> users)
        {
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

            foreach (var employee in users)
            {
                employee.PasswordHash = passwordHasher.HashPassword(employee, "Develop@90");
            }

            return users;
        }

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
