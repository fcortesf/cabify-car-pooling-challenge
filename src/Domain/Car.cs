using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace car_pooling_api.Domain {
    public class Car {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Seats { get; set; }

        public int EmptySeats { 
            get {
                return Seats - GroupsAssigned.Sum(e => e.People);
            } 
        }

        public ICollection<Group> GroupsAssigned { get; set; }

        public Car()
        {
            GroupsAssigned = new List<Group>();
        }

    }
}