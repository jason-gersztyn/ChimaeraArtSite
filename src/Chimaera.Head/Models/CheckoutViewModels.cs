using Chimaera.Beasts.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System;

namespace Chimaera.Head.Models
{
    public class CheckoutShippingViewModel
    {
        public Quote quote { get; set; }

        public int AddressID { get; set; }

        [Required(ErrorMessage = "Name required")]
        [StringLength(50, MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z' ]+$", ErrorMessage = "That's not a valid name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email required")]
        [StringLength(200, MinimumLength = 5)]
        [EmailAddress(ErrorMessage = "That's not a valid Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Street line 1 required")]
        [StringLength(200, MinimumLength = 1)]
        public string Street1 { get; set; }

        [StringLength(200)]
        public string Street2 { get; set; }

        [Required(ErrorMessage = "City required")]
        [StringLength(100, MinimumLength = 1)]
        public string City { get; set; }

        [Required(ErrorMessage = "State/Region required")]
        [StringLength(100, MinimumLength = 1)]
        public string State { get; set; }

        [Required(ErrorMessage = "Country required")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Postal code required")]
        [StringLength(50, MinimumLength = 1)]
        public string Zip { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }
    }

    public class CheckoutApprovedViewModel
    {
        public Quote quote { get; set; }
        public string PaymentID { get; set; }
        public string PayerID { get; set; }
    }

    public class CheckoutApprovedPostModel
    {
        [Required]
        public string QuoteKey { get; set; }
        [Required]
        public string PaymentID { get; set; }
        [Required]
        public string PayerID { get; set; }
    }

    public class CheckoutCancelledViewModel
    {
        public Order order { get; set; }
    }

    public class CheckoutCompleteViewModel
    {
        public Order order { get; set; }
    }

    public class CheckoutHistoryViewModel
    {
        public Order order { get; set; }

        public Shipment shipment { get; set; }
    }
}