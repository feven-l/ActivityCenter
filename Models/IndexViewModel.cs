using System.Collections.Generic;

namespace ActivityCenter.Models
{
    public class IndexViewModel
    {
        public User SingleUser {get;set;}

        public LoginUser SingleLogin {get;set;}

        public List<Plan> AllPlans {get;set;}

        public List<Plan> going {get;set;}
        public List<Plan> Notgoing {get;set;}

        public List<Plan> Creator {get;set;}

        public List<Plan> CList {get;set;}

        public Plan SinglePlan {get;set;}

        public Association NewAssos {get;set;}

        public int UserId {get;set;}

    }
}