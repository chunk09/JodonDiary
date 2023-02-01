using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JodonDiary.Models;

public class Diary
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    
    public string title { get; set; }
    public string content { get; set; }
    public string date { get; set; }
    public int? user_key { get; set; }
    public int private_check { get; set; }
}