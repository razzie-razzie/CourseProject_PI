﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CourseProject
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DinnersDBEntities : DbContext
    {
        public DinnersDBEntities()
            : base("name=DinnersDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Dish> Dishes { get; set; }
        public virtual DbSet<DishesType> DishesTypes { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<IngredientsCategory> IngredientsCategories { get; set; }
        public virtual DbSet<IngredientsOnPosition> IngredientsOnPositions { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<PositionsInDish> PositionsInDishes { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    }
}
