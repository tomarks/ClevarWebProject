using ClevarWeb.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClevarWeb.Data.SampleData
{
    public static class PeopleSamples
    {
        public static List<Person> AsList()
        {
            var people = new List<Person>();

            people.Add(new Person()
            {
                Name = "Ross",
                Title = "Grand Poobah",
                SubTitle = "At one with the virtual.",
                CartoonImages = new List<CartoonImage>()
                {
                    new CartoonImage()
                    {
                        LayerNumber = 1,
                        Styles = "clevar-animate-float clevar-static-glow",
                        Document =  Document.CreatePreDefinedDocument("samples/cartoons/Clevar_Ross_Blue.png")
                    },
                    new CartoonImage()
                    {
                        LayerNumber = 2,
                        Styles = "",
                        Document = Document.CreatePreDefinedDocument("samples/cartoons/Clevar_Ross_Main.png")
                    }
                },
                BioHtml = GetLorem()
            });

            people.Add(new Person()
            {
                Name = "Rodney",
                CartoonImages = new List<CartoonImage>()
                {
                    new CartoonImage()
                    {
                        LayerNumber = 1,
                        Styles = "clevar-animate-float clevar-static-glow",
                        Document = Document.CreatePreDefinedDocument("samples/cartoons/Clevar_Rodney_Blue.png")
                    },
                    new CartoonImage()
                    {
                        LayerNumber = 2,
                        Styles = "",
                        Document = Document.CreatePreDefinedDocument("samples/cartoons/Clevar_Rodney_Main.png")
                    }
                },
                BioHtml = GetLorem()
            });

            return people;
        }

        public static string GetLorem() => "<h2>Lorem</h2><p> Lorem ipsum dolor sit, amet consectetur adipisicing elit.Unde voluptatum modi repellendus reprehenderit veniam maxime veritatis ipsum, expedita inventore, maiores aut aliquam ab facere odit aperiam ipsam ad necessitatibus ducimus?</p><h3>Ipsum</h3><p>Lorem ipsum dolor sit amet consectetur adipisicing elit. Officiis impedit molestiae, dolore obcaecati laborum commodi sint harum id rem magnam?</p><p>Lorem, ipsum dolor sit amet consectetur adipisicing elit.Natus aspernatur ipsa voluptates voluptate deserunt odit, exercitationem eum laboriosam omnis. Quidem consequatur excepturi quia, delectus temporibus aut voluptatibus dolore nesciunt cum?</p><p>Lorem ipsum, dolor sit amet consectetur adipisicing elit. Voluptatibus ex velit maiores explicabo aut incidunt modi nobis assumenda at quod omnis dolorem dolores reiciendis nesciunt provident totam, magnam debitis. Earum!</p><button>Learn More!</button>";
    }
}
