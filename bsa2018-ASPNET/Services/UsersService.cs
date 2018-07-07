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

        #region LoadData

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

        #endregion

        #region Queries

        public List<(Post, int)> FirstQuery(int idUser)
        {
            var counts = Users.Where(u => u.Id == idUser)
                .First()
                .Posts.Select(p => (
                    Post: p,
                    CountComments: p.Comments.Count
                )).ToList();
            return counts;
        }

        public List<Comment> SecondQuery(int idUser)
        {
            var comments = Users.Where(u => u.Id == idUser)
                .SelectMany(u => u.Posts)
                .Where(p => p.Body.Length < 50)
                .SelectMany(p => p.Comments)
                .ToList();
            return comments;
        }

        public List<(int,string)> ThirdQuery(int idUser)
        {
            var toDos = Users.Where(u => u.Id == idUser)
                .SelectMany(u => u.ToDos)
                .Where(td => td.IsComplete)
                .Select(td => (Id: td.Id, Name: td.Name))
                .ToList();
            return toDos;
        }

        public List<User> FourthQuery()
        {
            var users = Users.OrderBy(u => u.Name)
                .Select(u => { u.ToDos = u.ToDos.OrderByDescending(td => td.Name.Length).ToList(); return u; })
                .ToList();
            return users;
        }

        public (User,Post,int,int,Post,Post) FifthQuery(int idUser)
        {
            var result = Users.Where(u => u.Id == idUser)
                .Select(u => (
                    User: u,
                    LastPost: u.Posts.OrderByDescending(c => c.CreateAt).FirstOrDefault(),
                    CommentsCount: 0,
                    CountUncompletedTodos: u.ToDos.Where(td => !td.IsComplete).Count(),
                    MaxCommentPost: u.Posts.Where(p => p.Body.Length > 80).OrderByDescending(p => p.Comments).FirstOrDefault(),
                    MaxLikesPost: u.Posts.OrderByDescending(p => p.Likes).FirstOrDefault()
                )).FirstOrDefault();
            result.CommentsCount = result.LastPost.Comments.Count;
            return result;
        }

        public (Post, Comment,Comment,int) SixthQuery(int idPost)
        {
            var result = Users.SelectMany(u => u.Posts)
                .Where(p => p.Id == idPost)
                .Select(p => (
                    Post: p,
                    LongestComment: p.Comments.OrderByDescending(c => c.Body).FirstOrDefault(),
                    LikestComment: p.Comments.OrderByDescending(c => c.Likes).FirstOrDefault(),
                    Count: p.Comments.Where(c => c.Likes == 0 || c.Body.Length < 80).Count()
                )).FirstOrDefault();
            return result;
        }

        #endregion
    }
}
