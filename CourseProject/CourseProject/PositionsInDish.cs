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
    
    public partial class PositionsInDish
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PositionsInDish()
        {
            this.IngredientsOnPositions = new HashSet<IngredientsOnPosition>();
        }
    
        public int ID { get; set; }
        public int DishID { get; set; }
        public int PositionID { get; set; }
    
        public virtual Dish Dish { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IngredientsOnPosition> IngredientsOnPositions { get; set; }
        public virtual Position Position { get; set; }
    }
}
