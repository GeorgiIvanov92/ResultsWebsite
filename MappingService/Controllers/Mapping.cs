using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace MappingService.Controllers
{
    
    public class Mapping : Controller
    {
        TrackerDBContext Context;
        public Mapping(TrackerDBContext context)
        {
            Context = context;
        }      

        public string GetLeague(string league)
        {
            if(league == null)
            {
                return "bad POST params";
            }
            var leagueArr = league.Split("@");
            if(leagueArr.Length != 2)
            {
                return "bad POST params";
            }
            var leagueName = leagueArr[0];
            int sportId;
            if(!int.TryParse(leagueArr[1],out sportId))
            {
                return "bad POST params";
            }
            var resultsFromSport = Context.Results.ToList().Where(res => res.SportId == sportId);
            switch (leagueName)
            {
                case "EUW":
                    return resultsFromSport.FirstOrDefault(res => res.LeagueName.ToLowerInvariant()
                    .Contains("european championship"))?.LeagueName
                        ?? "Could Not Locate League";
                case "KR":
                    return resultsFromSport.FirstOrDefault(res => res.LeagueName.ToLowerInvariant()
                    .Contains("champions korea"))?.LeagueName
                        ?? "Could Not Locate League";
                case "NA":
                    return resultsFromSport.FirstOrDefault(res => res.LeagueName.ToLowerInvariant()
                    .Contains("championship series"))?.LeagueName
                        ?? "Could Not Locate League";
                case "BR":
                    return resultsFromSport.FirstOrDefault(res => res.LeagueName.ToLowerInvariant()
                    .Contains("brasileiro"))?.LeagueName
                        ?? "Could Not Locate League";
                case "TR":
                    return resultsFromSport.FirstOrDefault(res => res.LeagueName.ToLowerInvariant()
                    .Contains("turkish"))?.LeagueName
                        ?? "Could Not Locate League";
                case "TW":
                    return resultsFromSport.FirstOrDefault(res => res.LeagueName.ToLowerInvariant()
                    .Contains("masters series"))?.LeagueName
                        ?? "Could Not Locate League";
                case "CIS":
                    return resultsFromSport.FirstOrDefault(res => res.LeagueName.ToLowerInvariant()
                    .Contains("continental league"))?.LeagueName
                        ?? "Could Not Locate League";
                case "OCE":
                    return resultsFromSport.FirstOrDefault(res => res.LeagueName.ToLowerInvariant()
                    .Contains("oceanic pro league"))?.LeagueName
                        ?? "Could Not Locate League";

            }
            return "Could Not Locate League";
        }
       
    }
}
