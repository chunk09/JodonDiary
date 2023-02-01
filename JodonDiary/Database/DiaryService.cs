using JodonDiary.Models;
using MySql.Data.MySqlClient;

namespace JodonDiary.Database;

public class DiaryService
{
    private const string connectString = "server=localhost;user=root;password=비밀번호;database=jodon";

    public bool ConnectionTest()
    {
        try
        {
            using var conn = new MySqlConnection(connectString);

            conn.Open();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public void InsertDiary(Diary diary)
    {
        var sql = $"INSERT INTO diary (title, content, date, user_key, private_check) VALUES ('{diary.title}', '{diary.content}', '{diary.date}', '{diary.user_key}', '{diary.private_check}')";

        using var conn = new MySqlConnection(connectString);

        try
        {
            conn.Open();

            var cmd = new MySqlCommand(sql, conn);

            if (cmd.ExecuteNonQuery() == 1)
            {
                Console.WriteLine("INSERT 성공");
            }
            else
            {
                Console.WriteLine("INSERT 실패");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("데이터베이스 연결 오류");
            Console.WriteLine(e);
        }
        
        conn.Close();
    }

    public List<Diary> GetDiary()
    {
        var list = new List<Diary>();
        var sql = "SELECT * FROM diary";

        using var conn = new MySqlConnection(connectString);

        conn.Open();

        var cmd = new MySqlCommand(sql, conn);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new Diary()
            {
                id = Convert.ToInt32(reader["id"]),
                title = reader["title"].ToString(),
                content = reader["content"].ToString(),
                date = reader["date"].ToString(),
                user_key = Convert.ToInt32(reader["user_key"]),
                private_check = Convert.ToInt16(reader["private_check"])
            });
        }

        conn.Close();

        return list;
    }
    
    public void DeleteDiary(int id)
    {
        var sql = $"DELETE FROM diary WHERE id = '{id}'";

        using var conn = new MySqlConnection(connectString);

        try
        {
            conn.Open();

            var cmd = new MySqlCommand(sql, conn);

            if (cmd.ExecuteNonQuery() == 1)
            {
                Console.WriteLine("DELETE 성공");
            }
            else
            {
                Console.WriteLine("DELETE 실패");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("데이터베이스 연결 오류");
            Console.WriteLine(e);
        }
        
        conn.Close();
    }
    
    public void UpdateData(int id, Diary diary)
    {
        var sql = $"Update diary Set title = '{diary.title}', content = '{diary.content}', date = '{diary.date}', user_key = '{diary.user_key}', private_check = '{diary.private_check}' where id = {id}";

        using var conn = new MySqlConnection(connectString);

        try
        {
            conn.Open();

            var cmd = new MySqlCommand(sql, conn);

            if (cmd.ExecuteNonQuery() == 1)
            {
                Console.WriteLine("Update 성공");
            }
            else
            {
                Console.WriteLine("Update 실패");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("DB Connection Fail!!!!");
            Console.WriteLine(e);
        }
        
        conn.Close();
    }
}