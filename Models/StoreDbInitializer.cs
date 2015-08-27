using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Store.Models
{
    public class StoreDbInitializer : DropCreateDatabaseIfModelChanges<StoreContext>
    {
        protected override void Seed(StoreContext db)
        {
            db.Category.Add(new Category {Name = "Аудиотехника", ParentId = 0});
            db.Category.Add(new Category { Name = "Ноутбуки и планшеты", ParentId = 0 });
            db.Category.Add(new Category { Name = "Игры и приставки", ParentId = 0 });
            db.Category.Add(new Category { Name = "Автотовары", ParentId = 0 });

            base.Seed(db);
        }
    }
}