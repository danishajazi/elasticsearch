using Elasticsearch.WebApp.Models;

namespace Elasticsearch.WebApp.Data
{
    public class MasterData
    {
        public static List<CheckBoxModel> GetColors()
        {
            return new List<CheckBoxModel>()
                {
                  new CheckBoxModel {Value=1,Text="Red",IsChecked=false },
                  new CheckBoxModel {Value=1,Text="Green",IsChecked=false },
                  new CheckBoxModel {Value=1,Text="Orange",IsChecked=false },
                  new CheckBoxModel {Value=1,Text="Blue" ,IsChecked=false},
                  new CheckBoxModel {Value=1,Text="Purple",IsChecked=false },
                  new CheckBoxModel {Value=1,Text="Yellow" ,IsChecked=false},
                };
        }

        public static List<CheckBoxModel> GetTags()
        {
            return new List<CheckBoxModel>()
                {
                  new CheckBoxModel {Value=1,Text="Mens wear",IsChecked=false },
                  new CheckBoxModel {Value=1,Text="Ladies Wear",IsChecked=false },
                  new CheckBoxModel {Value=1,Text="New Arraival",IsChecked=false },
                  new CheckBoxModel {Value=1,Text="Top Brands" ,IsChecked=false},
                  new CheckBoxModel {Value=1,Text="Kids Wear",IsChecked=false },
                  new CheckBoxModel {Value=1,Text="Jeans" ,IsChecked=false},
                };
        }
    }
}
