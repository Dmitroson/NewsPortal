using Business.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsPortal.ViewModels
{
    public class ArticleViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool Visibility { get; set; }

        public DateTime? PubDate { get; set; }

        public ArticleViewModel() { }

        public ArticleViewModel(Article article)
        {
            Id = article.Id;
            Title = article.Title;
            Description = article.Description;
            ImageUrl = article.ImageUrl;
            Visibility = article.Visibility;
            PubDate = article.PubDate;
        }
    }
}