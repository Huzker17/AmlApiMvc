using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmlApiMvc.Models
{
	public class NetworkType
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; init; }
		public string Name { get; init; }

		public NetworkType(string name)
		{
			Name = name;
		}
	}
}
