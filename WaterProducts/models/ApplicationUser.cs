using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WaterProducts.models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(255)]
        public string name { get; set; }

        public List<FavouriteProducts> favourites { get; set; }=new List<FavouriteProducts>();

        public List<Order> orders { get; set; } = new List<Order>();
        public Cart cart { get; set; }
    }
}
