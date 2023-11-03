﻿namespace Forum.Models
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public Guid Id { get; set; }

        [Required]
        public string Pseudonyme { get; set; }

        [Required]
        public string MotDePasse { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool Inscrit { get; set; }

        public bool Valide { get; set; }

        public string CheminAvatar { get; set; }

        public string Signature { get; set; }

        public bool Actif { get; set; } = true;

        public bool Admin { get; set; } = false;
    }
}
