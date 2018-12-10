//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Dish
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Dish()
        {
            this.PositionsInDishes = new HashSet<PositionsInDish>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public Nullable<double> Weight { get; set; }
        public Nullable<double> Cost { get; set; }
        public string ImageName { get; set; }
    
        public virtual DishesType DishesType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PositionsInDish> PositionsInDishes { get; set; }
    }
}
