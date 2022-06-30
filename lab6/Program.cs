using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace lab6
{
    class Program
    {
        private static Random r = new Random();
        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Выберите задание:\n" +
                                  "1)Ключи Диффи-Хеллмана\n" +
                                  "2)RSA-шифрование\n" +
                                  "3)Шифрование Эль-Гамаля\n" +
                                  "0)Выход");

                switch (Convert.ToInt32(Console.ReadLine()))
                {
                    case 1:
                        {
                            Console.WriteLine("---Ключи Диффи-Хеллмана---");
                            Console.WriteLine("Введите открытые числа p и g:");
                            DiffieHellman(Convert.ToInt32(Console.ReadLine()), Convert.ToInt32(Console.ReadLine()));
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("---RSA-шифрование---");
                            Console.WriteLine("Введите сообщение для шифрования:");
                            RSA(Console.ReadLine());
                            break;
                        }
                    case 3:
                        {
                            Console.WriteLine("---Шифрование Эль-Гамаля---");
                            Console.WriteLine("Введите сообщение для шифрования:");
                            int p = r.Next(5, 50);
                            int g = r.Next(1, p);
                            int x = r.Next(1, p);
                            crypt(p, g, x, Console.ReadLine());
                            break;
                        }
                    case 0:
                        {
                            exit = true;
                            break;
                        }
                    default:
                        Console.WriteLine("Неверный ввод");
                        break;
                }
            }

        }


        static int power(int a, int b, int n) // a^b mod n
        {
            int tmp = a;
            int sum = tmp;
            for (int i = 1; i < b; i++)
            {
                for (int j = 1; j < a; j++)
                {
                    sum += tmp;
                    if (sum >= n)
                        sum -= n;
                }

                tmp = sum;
            }

            return tmp;
        }

        static int mul(int a, int b, int n) // a*b mod n 
        {

            int sum = 0;

            for (int i = 0; i < b; i++)
            {
                sum += a;

                if (sum >= n)
                {
                    sum -= n;
                }
            }

            return sum;
        }

        static int modpow(int _base, int exp, int modulus)
        {
            _base %= modulus;
            int result = 1;
            while (exp > 0)
            {
                if ((exp & 1) != 0)
                    result = (result * _base) % modulus;
                _base = (_base * _base) % modulus;
                exp >>= 1;
            }

            return result;
        }

        static void DiffieHellman(int p, int g)
        {
            //Alice and Bob Key Gen
            int AliceSK = r.Next(1, 11); // Алиса генерирует свой закрытый ключ 
            int BobSK = r.Next(1, 11); // Боб генерирует свой закрытый ключ 

            int AliceOK =
                power(g, AliceSK, p); // С помощью своего числа, закрытого ключа и ключа Алиса генерирует открытый ключ
            int BobOK = power(g, BobSK, p); // С помощью своего числа, закрытого ключа и ключа Боб генерирует открытый ключ

            int AliceSessionKey = power(BobOK, AliceSK, p); //Алиса, получив открытый ключ Боба, генерирует ключ сессии
            int BobSessionKey = power(AliceOK, BobSK, p); //Боб, получив открытый ключ Алисы, генерирует ключ сессии

            if (AliceSessionKey == BobSessionKey)
            {
                Console.WriteLine($"Закрытый ключ Алисы: {AliceSK}");
                Console.WriteLine($"Открытый ключ Алисы: {AliceOK}");
                Console.WriteLine($"Закрытый ключ Боба: {BobSK}");
                Console.WriteLine($"Открытый ключ Боба: {BobOK}");
                Console.WriteLine($"Ключи сессии совпадают: {AliceSessionKey}");
            }
            else
            {
                Console.WriteLine("Ключи сессии не совпадают");
            }

        }

        static void RSA(string message)
        {
            int e = 17, n = 1073, d = 853;
            int[] encryptedMessage = new int[message.Length];
            for (int i = 0; i < message.Length; i++)
            {
                encryptedMessage[i] = modpow(message[i], e, n);
            }

            Console.WriteLine("--Зашифрованное сообщение--");
            for (int i = 0; i < encryptedMessage.Length; i++)
            {
                Console.Write(encryptedMessage[i]);
            }

            Console.WriteLine();

            char[] decryptedMessage = new char[encryptedMessage.Length];
            for (int i = 0; i < message.Length; i++)
            {
                decryptedMessage[i] = Convert.ToChar(modpow(encryptedMessage[i], d, n));
            }

            Console.WriteLine("--Расшифрованное сообщение--");
            Console.WriteLine(decryptedMessage);

        }

        static void crypt(int p, int g, int x, string message)
        {

            int y = power(g, x, p);

            Console.WriteLine($"Открытый ключ (p,g,y) = {p}, {g}, {y}");
            Console.WriteLine($"Закрытый ключ x = {x}");


            int k = r.Next(1, p); // 1 < k < (p-1) 
            int a = power(g, k, p);
            int M = Convert.ToInt32(message);
            int b = mul(power(y, k, p), M, p);


            Console.Write($"Шифротекст: \na:{a}\nb:{b}");

            Console.WriteLine();

            decrypt(a, p, x, b);
        }
        static void decrypt(int a, int p, int x, int b)
        {
            Console.WriteLine("--Расшированный текст--");
            Console.WriteLine($"{b * Math.Pow(a, p - 1 - x) % p}");

        }



    }

}