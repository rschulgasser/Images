using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pictures.data
{
   public class ImagesRepository
    {
        private string _connectionString;

        public ImagesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Image> GetAll()
        {
            using var context = new ImagesContext(_connectionString);
            return context.Images.OrderByDescending(o=>o.Date).ToList();
        }
        public void Add(Image image)
        {
            using var context = new ImagesContext(_connectionString);
            context.Images.Add(image);
            context.SaveChanges();
        }
        public Image GetById(int id)
        {
            using var context = new ImagesContext(_connectionString);
            return context.Images.FirstOrDefault(p => p.Id == id);
        }
        public void AddLike(Image image)
        {
            using var context = new ImagesContext(_connectionString);
            image.Likes++;
            context.Images.Attach(image);
            context.Entry(image).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
