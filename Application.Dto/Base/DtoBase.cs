using System;
using System.Collections.Generic;
using Infrastructure;
using Newtonsoft.Json;

namespace Application.Dto.Base
{
    public class DtoBase
    {
       private readonly List<ILink> _links = new List<ILink>();

        [JsonProperty(Order = 100)]
        public IList<ILink> Links { get { return _links; } }

        public void AddLink(ILink link)
        {
           _links.Add(link);
        }

        public void AddLinks(params ILink[] links)
        {
           Array.ForEach(links, AddLink);
        }
    }
}