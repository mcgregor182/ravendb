//-----------------------------------------------------------------------
// <copyright file="ExportSpec.cs" company="Hibernating Rhinos LTD">
//     Copyright (c) Hibernating Rhinos LTD. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Raven.Abstractions.Json;
using Raven.Json.Linq;

namespace Raven.Smuggler
{
	public class SmugglerOptions
	{
		public SmugglerOptions()
		{
			Filters = new Dictionary<string, string>();
			OperateOnTypes = ItemType.Indexes | ItemType.Documents | ItemType.Attachments;
		}

		public SmugglerAction Action { get; set; }

		public string File { get; set; }

		public Dictionary<string, string> Filters { get; set; }

		/// <summary>
		/// Specify the types to operate on. You can specify more than one type by combining items with the OR parameter.
		/// Default is all items.
		/// Usage example: OperateOnTypes = ItemType.Indexes | ItemType.Documents | ItemType.Attachments.
		/// </summary>
		public ItemType OperateOnTypes { get; set; }

		public bool MatchFilters(RavenJToken item)
		{
			foreach (var filter in Filters)
			{
				var copy = filter;
				foreach (var tuple in item.SelectTokenWithRavenSyntaxReturningFlatStructure(copy.Key))
				{
					if (tuple == null || tuple.Item1 == null)
						continue;
					var val = tuple.Item1.Type == JTokenType.String
								? tuple.Item1.Value<string>()
								: tuple.Item1.ToString(Formatting.None);
					if (string.Equals(val, filter.Value, StringComparison.InvariantCultureIgnoreCase) == false)
						return false;
				}
			}
			return true;
		}
	}

	[Flags]
	public enum ItemType
	{
		Documents,
		Indexes,
		Attachments,
	}

	public enum SmugglerAction
	{
		Import = 1,
		Export,
	}
}