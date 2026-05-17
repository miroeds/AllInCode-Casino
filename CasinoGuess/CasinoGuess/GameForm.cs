using System;
using System.Windows.Forms;

namespace CasinoGuess
{
    public class GameForm : Form
    {
        decimal balance = 1000;
        int minRange = 1;
        int maxRange = 10;
        int secretNumber;
        bool betPlaced = false;

        Label lblBalance;
        ComboBox cmbRange;
        TextBox txtBet;
        TextBox txtGuess;
        Button btnBet;
        Button btnGuess;
        Button btnReset;
        ListBox lstHistory;

        public GameForm()
        {
            CreateControls();
            UpdateBalanceDisplay();
        }

        private void CreateControls()
        {
            this.Text = "🎲 Казино — Угадай номер";
            this.Size = new System.Drawing.Size(520, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.ForeColor = System.Drawing.Color.White;

            lblBalance = new Label()
            {
                Text = $"💰 Баланс: {balance} ₽",
                Location = new System.Drawing.Point(30, 20),
                Size = new System.Drawing.Size(200, 30),
                Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.Gold
            };

            Label lblRange = new Label()
            {
                Text = "Диапазон чисел:",
                Location = new System.Drawing.Point(30, 70),
                Size = new System.Drawing.Size(120, 25)
            };

            cmbRange = new ComboBox()
            {
                Location = new System.Drawing.Point(150, 67),
                Size = new System.Drawing.Size(130, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRange.Items.AddRange(new object[] { "1–10 (легко)", "1–100 (сложно)" });
            cmbRange.SelectedIndex = 0;

            Label lblBet = new Label()
            {
                Text = "Сумма ставки (₽):",
                Location = new System.Drawing.Point(30, 110),
                Size = new System.Drawing.Size(120, 25)
            };

            txtBet = new TextBox()
            {
                Location = new System.Drawing.Point(150, 107),
                Size = new System.Drawing.Size(100, 25)
            };

            Label lblGuess = new Label()
            {
                Text = "Ваше число:",
                Location = new System.Drawing.Point(30, 150),
                Size = new System.Drawing.Size(120, 25)
            };

            txtGuess = new TextBox()
            {
                Location = new System.Drawing.Point(150, 147),
                Size = new System.Drawing.Size(100, 25),
                Enabled = false
            };

            btnBet = new Button()
            {
                Text = "🎲 Сделать ставку",
                Location = new System.Drawing.Point(30, 190),
                Size = new System.Drawing.Size(140, 40),
                BackColor = System.Drawing.Color.ForestGreen,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBet.Click += BtnBet_Click;

            btnGuess = new Button()
            {
                Text = "🎯 Угадать",
                Location = new System.Drawing.Point(180, 190),
                Size = new System.Drawing.Size(140, 40),
                BackColor = System.Drawing.Color.DodgerBlue,
                ForeColor = System.Drawing.Color.White,
                Enabled = false,
                FlatStyle = FlatStyle.Flat
            };
            btnGuess.Click += BtnGuess_Click;

            btnReset = new Button()
            {
                Text = "🔄 Новая игра",
                Location = new System.Drawing.Point(330, 190),
                Size = new System.Drawing.Size(140, 40),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnReset.Click += BtnReset_Click;

            lstHistory = new ListBox()
            {
                Location = new System.Drawing.Point(30, 250),
                Size = new System.Drawing.Size(440, 220),
                BackColor = System.Drawing.Color.FromArgb(45, 45, 45),
                ForeColor = System.Drawing.Color.LightGray
            };

            this.Controls.Add(lblBalance);
            this.Controls.Add(lblRange);
            this.Controls.Add(cmbRange);
            this.Controls.Add(lblBet);
            this.Controls.Add(txtBet);
            this.Controls.Add(lblGuess);
            this.Controls.Add(txtGuess);
            this.Controls.Add(btnBet);
            this.Controls.Add(btnGuess);
            this.Controls.Add(btnReset);
            this.Controls.Add(lstHistory);
        }

        private void UpdateBalanceDisplay()
        {
            lblBalance.Text = $"💰 Баланс: {balance} ₽";
            lblBalance.ForeColor = balance <= 0 ? System.Drawing.Color.Red : System.Drawing.Color.Gold;
        }

        private void BtnBet_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtBet.Text, out decimal bet) || bet <= 0)
            {
                MessageBox.Show("Введите сумму ставки (число > 0)", "Ошибка");
                return;
            }

            if (bet > balance)
            {
                MessageBox.Show($"Ставка {bet} ₽ больше баланса {balance} ₽", "Ошибка");
                return;
            }

            if (cmbRange.SelectedIndex == 0)
            { minRange = 1; maxRange = 10; }
            else
            { minRange = 1; maxRange = 100; }

            Random rand = new Random();
            secretNumber = rand.Next(minRange, maxRange + 1);

            txtBet.Enabled = false;
            cmbRange.Enabled = false;
            btnBet.Enabled = false;
            txtGuess.Enabled = true;
            btnGuess.Enabled = true;

            betPlaced = true;
            lstHistory.Items.Add($"👇 Ставка {bet} ₽. Диапазон {minRange}–{maxRange}. Угадывайте!");
        }

        private void BtnGuess_Click(object sender, EventArgs e)
        {
            if (!betPlaced)
            {
                MessageBox.Show("Сначала сделайте ставку", "Внимание");
                return;
            }

            if (!int.TryParse(txtGuess.Text, out int userNumber))
            {
                MessageBox.Show($"Введите число от {minRange} до {maxRange}", "Ошибка");
                return;
            }

            if (userNumber < minRange || userNumber > maxRange)
            {
                MessageBox.Show($"Число должно быть от {minRange} до {maxRange}", "Ошибка");
                return;
            }

            decimal bet = decimal.Parse(txtBet.Text);

            if (userNumber == secretNumber)
            {
                decimal win = bet + (bet * 0.5m);
                balance += win;
                lstHistory.Items.Add($"✅ ПОБЕДА! Число {secretNumber}. Выигрыш +{win} ₽. Баланс {balance} ₽");
                MessageBox.Show($"🎉 ПОБЕДА! Вы угадали {secretNumber}!\nВыигрыш: +{win} ₽", "Победа");
            }
            else
            {
                balance -= bet;
                lstHistory.Items.Add($"❌ ПРОИГРЫШ. Было {secretNumber}. Потеряно -{bet} ₽. Баланс {balance} ₽");
                MessageBox.Show($"😞 ПРОИГРЫШ. Было загадано {secretNumber}", "Проигрыш");
            }

            UpdateBalanceDisplay();

            if (balance <= 0)
            {
                MessageBox.Show("💀 Баланс закончился. Игра окончена.", "Game Over");
                btnGuess.Enabled = false;
                txtGuess.Enabled = false;
                return;
            }

            txtBet.Enabled = true;
            cmbRange.Enabled = true;
            btnBet.Enabled = true;
            txtGuess.Enabled = false;
            btnGuess.Enabled = false;
            txtGuess.Clear();
            txtBet.Clear();
            betPlaced = false;
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            balance = 1000;
            UpdateBalanceDisplay();

            txtBet.Enabled = true;
            cmbRange.Enabled = true;
            btnBet.Enabled = true;
            txtGuess.Enabled = false;
            btnGuess.Enabled = false;
            txtGuess.Clear();
            txtBet.Clear();
            lstHistory.Items.Clear();

            betPlaced = false;
            MessageBox.Show("🔄 Новая игра. Баланс 1000 ₽", "Новая игра");
        }
    }
}