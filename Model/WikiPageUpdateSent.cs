
    
    
    
    
    
// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
//      Generated: Sun, 15 Jan 2023 23:38:58 GMT
// </auto-generated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
namespace WikiPageApp.Model
{
	public class WikiPageUpdateSent : IEntity
	{
		public WikiPageUpdateSent() {
			WikiPageUpdateSentUpdatedTimestampUTC = DateTime.UtcNow;
			WikiPageUpdateSentIsSoftDeleted = false;
		}

		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public Guid WikiPageUpdateSentID { get; set; } = Guid.NewGuid();
		
		//Definition Properties
		

		public Guid UserID { get; set; }
		

		public Guid WikiPageUpdateID { get; set; }
		 
		[Column(TypeName = "bit")] 
		public bool WikiPageUpdateSentIsSoftDeleted { get; set; }
		public DateTime WikiPageUpdateSentUpdatedTimestampUTC { get; set; }
		public Guid WikiPageUpdateSentUpdatedByResourceID { get; set; }
 
	}
}