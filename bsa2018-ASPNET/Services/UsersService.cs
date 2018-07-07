using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using bsa2018_ASPNET.Models;
using Newtonsoft.Json;

namespace bsa2018_ASPNET.Services
{
    public class UsersService
    {
        public List<User> Users { get; set; }

        public UsersService()
        {
            if (Users == null)
                Users=LoadAsync().Result;
        }

        private async Task<List<User>> LoadAsync()
        {
            string page = "https://5b128555d50a5c0014ef1204.mockapi.io/";
            List<User> users;
            List<Post> posts;
            List<Comment> comments;
            List<Address> address;
            List<ToDo> toDos;
            using (HttpClient client = new HttpClient())
            {
                posts = await LoadAllPosts(client, page);
                comments = await LoadAllComments(client, page);
                address = await LoadAllAddress(client, page);
                toDos = await LoadAllToDos(client, page);
                users = await LoadAllUsers(client, page);
            }
            posts = (from post in posts
                     join comment in comments on post.Id equals comment.PostId into commentsGroup
                     select new Post
                     {
                         Id = post.Id,
                         Body = post.Body,
                         Comments = commentsGroup.ToList(),
                         CreateAt = post.CreateAt,
                         Likes = post.Likes,
                         Title = post.Title,
                         UserId = post.UserId
                     }).ToList();

            users = (from user in users
                     join post in posts on user.Id equals post.UserId into postsGroup
                     join toDo in toDos on user.Id equals toDo.UserId into toDosGroup
                     join comment in comments on user.Id equals comment.UserId into toCommentsGroup
                     join addres in address on user.Id equals addres.UserId
                     select new User
                     {
                         Id = user.Id,
                         Name = user.Name,
                         CreateAt = user.CreateAt,
                         Avatar = user.Avatar,
                         Email = user.Email,
                         Address = addres,
                         Posts = postsGroup.ToList(),
                         ToDos = toDosGroup.ToList(),
                         Comments = toCommentsGroup.ToList()
                     }).ToList();
            return users;
        }

        private async Task<List<User>> LoadAllUsers(HttpClient client, string page)
        {
            List<User> users;
            using (HttpResponseMessage response = await client.GetAsync(page + "users"))
            {
                string result = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<User>>(result);
            }
            return users;
        }

        private async Task<List<ToDo>> LoadAllToDos(HttpClient client, string page)
        {
            List<ToDo> toDos;
            using (HttpResponseMessage response = await client.GetAsync(page + "todos"))
            {
                string result = await response.Content.ReadAsStringAsync();
                toDos = JsonConvert.DeserializeObject<List<ToDo>>(result);
            }
            return toDos;
        }

        private async Task<List<Address>> LoadAllAddress(HttpClient client, string page)
        {
            List<Address> address;
            using (HttpResponseMessage response = await client.GetAsync(page + "address"))
            {
                string result = await response.Content.ReadAsStringAsync();
                address = JsonConvert.DeserializeObject<List<Address>>(result);
            }
            return address;
        }

        private async Task<List<Post>> LoadAllPosts(HttpClient client, string page)
        {
            List<Post> posts;
            using (HttpResponseMessage response = await client.GetAsync(page + "posts"))
            {
                string result = await response.Content.ReadAsStringAsync();
                posts = JsonConvert.DeserializeObject<List<Post>>(result);
            }
            return posts;
        }

        private async Task<List<Comment>> LoadAllComments(HttpClient client, string page)
        {
            List<Comment> comments;
            using (HttpResponseMessage response = await client.GetAsync(page + "comments"))
            {
                string result = await response.Content.ReadAsStringAsync();
                comments = JsonConvert.DeserializeObject<List<Comment>>(result);
            }
            return comments;
        }
    }
}
