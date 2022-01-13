using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClevarWeb.Data.Models;
using ClevarWeb.Pages.Admin.People;
using ClevarWeb.Pages.Admin.Projects;
using ClevarWeb.Pages.Admin.Publications;

namespace ClevarWeb.Data
{
    public class AutoMapping: Profile
    {
        public AutoMapping()
        {
            CreateMap<Project, EditProjectModel>().ReverseMap();
            CreateMap<Person, EditPeopleModel>().ReverseMap();
            CreateMap<Publication, EditPublicationModel>().ReverseMap();
        }
    }
}
