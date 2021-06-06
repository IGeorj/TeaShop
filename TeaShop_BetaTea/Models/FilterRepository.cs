using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace TeaShop_BetaTea.Models
{
    public class FilterRepository
    {
        public FilterRepository()
        {
            sizeList = new SelectList( new List<string>{ "Крупный лист", "Средний лист", "Мелкий лист" });
            colorList = new SelectList( new List<string>{ "Черный чай", "Зеленый чай", "Белый чай", "Красный чай", "Синий чай"});
            typeList = new SelectList( new List<string>{ "Гранулированный", "Листовой", "Пакетированный", "Растворимый", "Молотый" });
            Dictionary<string, string> cultureList = new Dictionary<string, string>();
            CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo getCulture in getCultureInfo)
            {
                RegionInfo getRegionInfo = new RegionInfo(getCulture.LCID);
                if (!(cultureList.ContainsKey(getRegionInfo.Name)))
                {
                    cultureList.Add(getRegionInfo.Name, getRegionInfo.DisplayName);
                }
            }
            countryList = new SelectList(cultureList.Values);
        }

        private SelectList sizeList;

        public SelectList SizeList
        {
            get
            {
                return sizeList;
            }
        }

        private SelectList colorList;

        public SelectList ColorList
        {
            get
            {
                return colorList;
            }
        }

        private SelectList brandList;

        public SelectList BrandList
        {
            get
            {
                return brandList;
            }
            set
            {
                brandList = value;
            }
        }
        private SelectList typeList;

        public SelectList TypeList
        {
            get
            {
                return typeList;
            }
        }
        private SelectList countryList;

        public SelectList CountryList
        {
            get
            {
                return countryList;
            }
        }
    }
}