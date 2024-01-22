using Microsoft.Data.SqlClient;

namespace DZ_19_01
{
    public partial class Form1 : Form
    {
        private SqlConnection connection = new SqlConnection("Server=LIBERTY; Database=DZ_19_01; Trusted_Connection=True; TrustServerCertificate=True; Integrated Security=True;");
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection("Server=LIBERTY; Database=DZ_19_01; Trusted_Connection=True; TrustServerCertificate=True; Integrated Security=True;"))
            {
                try
                {
                    int sourceAccount = Convert.ToInt32(textBox1.Text);
                    int destinationAccount = Convert.ToInt32(textBox2.Text);
                    decimal transferAmount = Convert.ToDecimal(textBox3.Text);

                    bool transactionResult = PerformTransaction(sourceAccount, destinationAccount, transferAmount);

                    if (transactionResult)
                    {
                        textBox4.Text = "Транзакція успішна!";
                    }
                    else
                    {
                        textBox4.Text = "Помилка транзакції.";
                    }
                }
                catch (Exception ex)
                {
                    textBox4.Text = "Помилка: " + ex.Message;
                }
            }
        }
        private bool PerformTransaction(int sourceAccount, int destinationAccount, decimal amount)
        {
            try
            {
                connection.Open();

                string updateQuery = "UPDATE Accounts SET Balance = Balance - @amount WHERE AccountNumber = @sourceAccount;" +
                                     "UPDATE Accounts SET Balance = Balance + @amount WHERE AccountNumber = @destinationAccount;";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@amount", amount);
                    command.Parameters.AddWithValue("@sourceAccount", sourceAccount);
                    command.Parameters.AddWithValue("@destinationAccount", destinationAccount);

                    command.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
