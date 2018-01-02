using ParticleSwarmOptimization.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ParticleSwarmOptimization
{
    public partial class Main : Form
    {
        private PSO pso;
        private OptimizationProblem optimizationProblem;

        #region Windows Form Settings
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        #endregion

        public Main()
        {
            InitializeComponent();

            // Problem definition
            optimizationProblem = new OptimizationProblem(Problems.Function, OptimizationProblemType.Minimization);
            optimizationProblem.AddVariable("x1", 1, 15, DecisionVariableType.Integer);
            optimizationProblem.AddVariable("x2", 1, 15, DecisionVariableType.Integer);
            optimizationProblem.AddVariable("x3", 1, 15, DecisionVariableType.Integer);
        }

        #region Events
        private void PSO_OnStart(object sender)
        {
            BtnRun.BackColor = Color.YellowGreen;
            BtnRun.Text = "Runnig ...";
            BtnRun.Enabled = false;
            SettingsGroupBox.Enabled = false;
            TbxOutput.Text = "PSO alghorithm started." + Environment.NewLine;
        }

        private void PSO_OnIteration(object sender, IterationInfo e)
        {
            if (e.Iteration == 0 || e.Iteration == pso.Iterations - 1 || (e.Iteration + 1) % 10 == 0)
                TbxOutput.AppendText(string.Format("Iteration {0:D4} | Best Objective = {1}", e.Iteration + 1, e.BestObjectiveValue) + Environment.NewLine);
        }

        private void PSO_OnEnd(object sender)
        {
            TbxOutput.AppendText("PSO alghorithm finished." + Environment.NewLine);
            TbxOutput.AppendText(string.Format("{0}Final solution:{0}", Environment.NewLine));

            double[] temp = pso.BestSolution.ActualPosition;

            for (int i = 0; i < temp.Length; i++)
            {
                TbxOutput.AppendText(string.Format("\t{0}.    {1}", i + 1, temp[i]) + Environment.NewLine);
            }

            BtnRun.Enabled = true;
            SettingsGroupBox.Enabled = true;
            BtnRun.BackColor = Color.Silver;
            BtnRun.Text = "Start";
        }
        #endregion

        #region Controls
        private void MainPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void Main_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Logo_DoubleClick(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void BtnRun_Click(object sender, EventArgs e)
        {
            pso = new PSO((int)nudPopulation.Value, (int)nudItrations.Value);
            pso.SetData(((double)nudWeight.Value, (double)nudDamp.Value, new double[] { (double)nudCoefficients1.Value, (double)nudCoefficients2.Value }));

            // Event handlers
            pso.OnStart += PSO_OnStart;
            pso.OnIteration += PSO_OnIteration;
            pso.OnEnd += PSO_OnEnd;

            pso.Run(optimizationProblem);
        }
        #endregion
    }
}