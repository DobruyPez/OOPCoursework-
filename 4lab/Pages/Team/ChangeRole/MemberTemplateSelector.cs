using System.Windows;
using System.Windows.Controls;
using Roles;
using static Roles.CurrentTeam;

namespace _4lab.Pages.Team.ChangeRole
{
    public class MemberTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OwnerTemplate { get; set; }
        public DataTemplate EditableTemplate { get; set; }
        public DataTemplate ReadOnlyTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is TeamPage.MemberViewModel member)
            {
                if (member.IsOwner)
                    return OwnerTemplate;

                var currentUser = CurrentUser.Instance.GetCurrentUser();
                var isCurrentUserOwner = CurrentTeam.Team?.OwnerId == currentUser?.Id;

                return isCurrentUserOwner ? EditableTemplate : ReadOnlyTemplate;
            }
            return ReadOnlyTemplate;
        }
    }
}