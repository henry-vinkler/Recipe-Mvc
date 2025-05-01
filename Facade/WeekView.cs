using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecipeMvc.Facade;

[DisplayName("Week")]
public class WeekView : EntityView
{
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
}