﻿using System.ComponentModel.DataAnnotations;

namespace BlazorAPI.ApiRequest.Model
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название обязательно для заполнения")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Описание обязательно для заполнения")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Жанр обязателен для заполнения")]
        public string Genre { get; set; }

        [Required(ErrorMessage = "Дата выхода обязательна для заполнения")]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "Рейтинг обязателен для заполнения")]
        [Range(0, 10, ErrorMessage = "Рейтинг должен быть от 0 до 10")]
        public decimal Rating { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
