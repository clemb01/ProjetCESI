using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Core
{
    public class Ressource : EntiteBase
    {
        [DataMember]
        public DateTimeOffset DateCreation { get; set; }
        [DataMember]
        public DateTimeOffset DateModification { get; set; }
        [DataMember]
        public DateTimeOffset DateSuppression { get; set; }
        [DataMember]
        public string Titre { get; set; }
        [DataMember]
        public string Contenu { get; set; }
        [DataMember]
        public string ContenuOriginal { get; set; }
        [DataMember]
        public Statut Statut { get; set; }
        [DataMember]
        public bool RessourceSupprime { get; set; }
        [DataMember]
        public int NombreConsultation { get; set; }
        [DataMember]
        public TypePartage TypePartage { get; set; }

        [DataMember]
        public Ressource RessourceParent { get; set; }
        [DataMember]
        public int? RessourceParentId { get; set; }
        [DataMember]
        public User UtilisateurCreateur { get; set; }
        [DataMember]
        public int? UtilisateurCreateurId { get; set; }
        [DataMember]
        public TypeRessource TypeRessource { get; set; }
        [DataMember]
        public int? TypeRessourceId { get; set; }
        [DataMember]
        public Categorie Categorie { get; set; }
        [DataMember]
        public int? CategorieId { get; set; }
        [NotMapped]
        public string ShareLink { get; set; }
        [DataMember]
        public string KeyLink { get; set; }

        public List<UtilisateurRessource> UtilisateurRessources { get; set; }
        public List<TypeRelationRessource> TypeRelationsRessources { get; set; }
        public List<Commentaire> Commentaires { get; set; }
        public List<Ressource> HistoriqueRessource { get; set; }

        public Ressource Clone()
        {
            Ressource clone = new Ressource
            {
                Id = default,
                TypeRessourceId = this.TypeRessourceId,
                CategorieId = this.CategorieId,
                UtilisateurCreateurId = this.UtilisateurCreateurId,
                DateCreation = this.DateCreation,
                DateModification = this.DateModification,
                DateSuppression = this.DateSuppression,
                NombreConsultation = this.NombreConsultation,
                Statut = this.Statut,
                Titre = this.Titre,
                Contenu = this.Contenu,
                ContenuOriginal = this.ContenuOriginal,
                RessourceSupprime = this.RessourceSupprime,
                TypePartage = this.TypePartage,
                RessourceParentId = this.RessourceParentId
            };

            return clone;
        }
    }

    public enum StatutActivite
    {
        [Display(Name = "Non démarré")]
        NonDemare,
        [Display(Name = "Démarré")]
        Demare,
        [Display(Name = "En pause")]
        EnPause,
        [Display(Name = "Terminé")]
        Termine
    }

    public enum Statut
    {
        [Display(Name = "Vide")]
        Empty,
        [Display(Name = "En pause")]
        EnPause,
        [Display(Name = "En attente de validation")]
        AttenteValidation,
        [Display(Name = "Accepté")]
        Accepter,
        [Display(Name = "Refusé")]
        Refuser,
        [Display(Name = "Suspendu")]
        Suspendre
    }

    public enum TypePartage
    {
        Public,
        Partage,
        Prive
    }

    public enum TypeTriBase
    {
        [Display(Name = "Du plus ancien au plus récent")]
        DateModification,
        [Display(Name = "Du plus récent au plus ancien")]
        DateModificationDesc,
        [Display(Name = "De A à Z")]
        Titre,
        [Display(Name = "De Z à A")]
        TitreDesc,
        [Display(Name = "Les moins consultés")]
        NombreConsultation,
        [Display(Name = "Les plus consultés")]
        NombreConsultationDesc
    }
}