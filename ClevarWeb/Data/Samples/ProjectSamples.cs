using System;
using System.Collections.Generic;
using System.Linq;
using ClevarWeb.Data.Models;

namespace ClevarWeb.Data.SampleData
{
    public static class ProjectSamples
    {
        public static List<Project> AsList()
        {
            List<Project> projects = new List<Project>();

            projects.Add(new Project()
            {
                Title = "The Effects of Virtual Environment on Arousal",
                PrimaryDocument = Document.CreatePreDefinedDocument("samples/projects/AR Medical 1.jpg"),
                Description = "In this project, participants first enter the Richie’s Plank experience. This experience is intended to affect their level of arousal. After this experience, participants will enter one of three virtual worlds, or just wait without entering another virtual world. Using both quantitative and qualitative measures, the effect that the second condition has on arousal is measured. This project is the analysis of what specific virtual world qualities have on arousal.",
                HTMLContent = "<p>Html content goes here.</p>",
                IsActive = true,
                PublishedDateTime = DateTime.Parse("2020-04-10 12:00")
            }) ;

            projects.Add(new Project()
            {
                Title = "Vague Gesture Control (PhD Project)",
                PrimaryDocument = Document.CreatePreDefinedDocument("samples/projects/Vague Gesture Control.png"),
                Tagline = "The effects of Anticipation of Clarifying Unclear Gestures within Burns Rehabilitation.",
                Description = "Within burns rehabilitation, hands can be damaged or bandaged, which can lead to the results of visual hand detection being less defined. Additionally, distance from the detecting camera can dramatically affect gesture recognition. This project aims to use anticipation of gestures to be clarifying these vague, or less distinct, hand gestures. This is accomplished by first using a Convolutional Neural Network to recognised hand gesture images captured by a RGB-D camera. Then a Long Short Term Memory Neural Network anticipates gestures based on prior learning. The purpose of this project is to attempt to improve gesture detection in burns rehabilitation, while providing automated range of motion data for occupational therapists. Picture shows examples of artificial hands developed for training of a neural network.",
                HTMLContent = "<p>Html content goes here.</p>",
                IsActive = true,
                PublishedDateTime = DateTime.Parse("2020-04-12 12:00")
            });
            
            projects.Add(new Project()
            {
                PrimaryDocument = Document.CreatePreDefinedDocument("samples/projects/DesignBoxSessions.png"),
                Title = "Robert Cuthbert (PhD Project)",
                Tagline = "Designing Virtual Reality Environments for Burns Rehabilitation.",
                Description = "Burns rehabilitation has a high drop out rate. Virtual Reality (VR) has been shown to be more effective than conventional techniques for rehabilitation in part due to greater feelings of presence, immersion an engagement. Using virtual environments, this project applies gamification techniques such as providing feeling of competence, relatedness and autonomy, to increase motivation for the continuation of rehabilitation programs. An Occulus Quest will be used as an easy-to-use VR system, with the developed remedial program being constructed using Unreal Engine. The design of the system in done in conjunction with all the stakeholders involved in rehabilitation; that being patients, occupational therapists, doctors and administration. A design box paradigm is used to gather rich information.",
                HTMLContent = "<p>Html content goes here.</p>",
                IsActive = true,
                PublishedDateTime = DateTime.Parse("2020-04-11 12:00")
            });

            return projects;
        }
    }
}



