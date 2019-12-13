using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ActivityCenter.Models
{
    public class Association
    {
        [Key]
        public int AssociationId {get;set;}
        public int UserId {get;set;}
        public int PlanId {get;set;}
        public User user {get;set;}
        public Plan plan {get;set;}

    }
}