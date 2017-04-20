using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simples
{
    class Program
    {
        static void Main(string[] args)
        {
            Process p = new Process();
            Console.WriteLine("Estado inicial = " + p.EstadoAtual);
            Console.WriteLine("Iniciar: Estado atual = " + p.MoverPara(Comando.Iniciar));
            Console.WriteLine("Pausar: Estado atual = " + p.MoverPara(Comando.Pausar));
            Console.WriteLine("Terminar: Estado atual = " + p.MoverPara(Comando.Terminar));
            Console.WriteLine("Sair: Estado atual = " + p.MoverPara(Comando.Sair));
            Console.ReadLine();
        }
    }
    #region itens da maquina
    public enum Estado
    {
        Inativo,
        Ativo,
        Pausado,
        Terminado
    }

    public enum Comando
    {
        Iniciar,
        Terminar,
        Pausar,
        Retomar,
        Sair
    }

    public class Process
    {
        class StateTransition
        {
            readonly Estado EstadoAtual;
            readonly Comando Comando;

            public StateTransition(Estado estadoAtual, Comando command)
            {
                EstadoAtual = estadoAtual;
                Comando = command;
            }

            public override int GetHashCode()
            {
                return 10 * EstadoAtual.GetHashCode() + 100 * Comando.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                StateTransition other = obj as StateTransition;
                return other != null && this.EstadoAtual == other.EstadoAtual && this.Comando == other.Comando;
            }
        }

        Dictionary<StateTransition, Estado> transitions;
        public Estado EstadoAtual { get; private set; }

        public Process()
        {
            EstadoAtual = Estado.Inativo;
            //Aqui são definidas as possíveis transições de estado
            transitions = new Dictionary<StateTransition, Estado>
            {
                { new StateTransition(Estado.Inativo, Comando.Sair), Estado.Terminado },
                { new StateTransition(Estado.Inativo, Comando.Iniciar), Estado.Ativo },
                { new StateTransition(Estado.Ativo, Comando.Terminar), Estado.Inativo },
                { new StateTransition(Estado.Ativo, Comando.Pausar), Estado.Pausado },
                { new StateTransition(Estado.Pausado, Comando.Terminar), Estado.Inativo },
                { new StateTransition(Estado.Pausado, Comando.Retomar), Estado.Ativo }
            };
        }

        public Estado GetNovoEstado(Comando comando)
        {
            StateTransition transition = new StateTransition(EstadoAtual, comando);
            Estado proximoEstado;
            if (!transitions.TryGetValue(transition, out proximoEstado))
                throw new Exception("Invalid transition: " + EstadoAtual + " -> " + comando);
            return proximoEstado;
        }

        public Estado MoverPara(Comando comando)
        {
            EstadoAtual = GetNovoEstado(comando);
            return EstadoAtual;
        }
    }
    #endregion

}
