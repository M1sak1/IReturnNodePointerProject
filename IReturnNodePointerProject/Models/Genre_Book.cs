﻿using System.ComponentModel.DataAnnotations;

namespace IReturnNodePointerProject.Models
{
	public class Book_genre
	{
		[Key]
		public int subGenreID { get; set; }
		public string Name { get; set; }
	}
}
