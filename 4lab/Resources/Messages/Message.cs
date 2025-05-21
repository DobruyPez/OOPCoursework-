using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Roles;

namespace _4lab.Resources
{
    public enum MessageType
    {
        [Display(Name = "none")]
        Default,

        [Display(Name = "team_invitation")]
        TeamInvitation,

        [Display(Name = "team_offer")]
        TeamOffer
    }

    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }

        [Required]
        [ForeignKey("Sender")]
        public int SenderId { get; set; }

        [Required]
        [ForeignKey("Receiver")]
        public int ReceiverId { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string Content { get; set; }

        [Required]
        public bool IsRead { get; set; } = false;

        [Required]
        [Column("message_type", TypeName = "varchar(20)")] 
        public string MessageTypeString { get; set; } = MessageType.Default.ToString();

        [NotMapped]
        public MessageType MessageType
        {
            get => (MessageType)Enum.Parse(typeof(MessageType), MessageTypeString);
            set => MessageTypeString = value.ToString();
        }

        [ForeignKey("TeamOffer")]
        public int? OfferId { get; set; }  // Nullable, так как не все сообщения связаны с оферами

        public virtual TeamOffer TeamOffer { get; set; }

        // Навигационные свойства
        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
    }
}