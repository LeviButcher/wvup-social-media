using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.Models;
using WVUPSM.Models.Entities;

namespace WVUPSM.DAL.Initiliazers
{
    public static class SampleData
    {
        public static IEnumerable<User> GetUsers()
        {
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            List<User> userList = new List<User>();
            User levi = new User()
            {
                Email = "leviB@Develop.com",
                NormalizedEmail = "LEVIB@DEVELOP.COM",
                UserName = "leviB",
                NormalizedUserName = "LEVIB",
                SecurityStamp = Guid.NewGuid().ToString(),
                Posts = new List<Post>()
                {
                    new Post()
                    {
                        Text = "I like cats more then dogs, fight me"
                    }
                }
            };
            User sean = new User()
            {
                Email = "seanR@Develop.com",
                NormalizedEmail = "seanR@DEVELOP.COM",
                UserName = "seanR",
                NormalizedUserName = "SEANR",
                SecurityStamp = Guid.NewGuid().ToString(),
                Posts = new List<Post>()
                {
                    new Post()
                    {
                        Text = "I should really play link to the past becuase it's" +
                        "the best zelder game of all time. #OcarinaBLOWS"
                    },
                    new Post()
                    {
                        Text = "This platform is way better then the ZUCKS. #ZUCKSSUCKS"
                    }
                }
            };
            User sam = new User()
            {
                Email = "samB@Develop.com",
                NormalizedEmail = "SAMB@DEVELOP.COM",
                UserName = "samB",
                NormalizedUserName = "SAMB",
                SecurityStamp = Guid.NewGuid().ToString(),
                Posts = new List<Post>()
                {
                    new Post()
                    {
                        Text = "GEN 2 is the best generation. no arguements"
                    },
                    new Post()
                    {
                        Text = "Zuckerberg can't even compete, #WeSellAllCustomerDataNotSome"
                    }
                }
            };

            userList.Add(levi);
            userList.Add(sean);
            userList.Add(sam);

            return SetPasswords(userList);
        }

        public static IEnumerable<Follow> GetFollowing(List<User> users) =>
            new List<Follow>()
            {
                new Follow()
                {
                    User = users.Find(x => x.UserName == "leviB"),
                    Person = users.Find(x => x.UserName == "samB"),
                },
                new Follow()
                {
                    User = users.Find(x => x.UserName == "leviB"),
                    Person = users.Find(x => x.UserName == "seanR"),
                },
                new Follow()
                {
                    User = users.Find(x => x.UserName == "seanR"),
                    Person = users.Find(x => x.UserName == "leviB"),
                },
                new Follow()
                {
                    User = users.Find(x => x.UserName == "samB"),
                    Person = users.Find(x => x.UserName == "seanR"),
                },
                new Follow()
                {
                    User = users.Find(x => x.UserName == "samB"),
                    Person = users.Find(x => x.UserName == "leviB"),
                }
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
    }
}
