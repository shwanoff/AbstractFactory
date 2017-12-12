using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractFactory
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Добро пожаловать на 76-ые Космические игры...");
			Console.ReadLine();

			var piratShip = new Spaceship("Навуходоносор", new PiratSpaceshipFactory());
			var warShip = new Spaceship("Ностромо", new WarSpaceshipFactory());

			Console.WriteLine($"Первый претендент: {piratShip}");
			Console.WriteLine($"Второй претендент: {warShip}");
			Console.ReadLine();

			var battle = new Battle(piratShip, warShip);

			Console.WriteLine("Поехали!");
			Console.ReadLine();

			var raceWinner = battle.Race();
			if(raceWinner != null)
			{
				Console.WriteLine($"Поприветствуйте победителя гонки {raceWinner}");
			}
			else
			{
				Console.WriteLine("В гонке ничья");
			}
			Console.ReadLine();

			Console.WriteLine("Да начнется смертельная битва!");
			Console.ReadLine();

			var battleWinner = battle.Fight();
			if (battleWinner != null)
			{
				Console.WriteLine($"Поприветствуйте победителя сражения {battleWinner}");
			}
			else
			{
				Console.WriteLine("В битве ничья");
			}
			Console.ReadLine();
		}
	}

	/// <summary>
	/// Класс описывающий Абстрактную фабрику.
	/// </summary>
	/// <remarks>
	/// Описываются семейства взаимосвязанных классов для создания космического корабля.
	/// </remarks>
	public abstract class SpaceshipFactory
	{
		/// <summary>
		/// Запас здоровья космического корабля.
		/// </summary>
		public int Health { get; protected set; } = 300;

		/// <summary>
		/// Тип космического корабля.
		/// </summary>
		public string Type { get; protected set; } = "Космический корабль";

		/// <summary>
		/// Создать двигатель космического корабля.
		/// </summary>
		/// <returns>Двигатель.</returns>
		public abstract Engine CreateEngine();

		/// <summary>
		/// Создать оружие космического корабля.
		/// </summary>
		/// <returns>Оружие.</returns>
		public abstract Gun CreateGun();

		/// <summary>
		/// Создать источник энергии космического корабля.
		/// </summary>
		/// <returns>Источник энергии.</returns>
		public abstract Energy CreateEnergy();
	}

	#region Energy
	/// <summary>
	/// Базовый абстрактный класс описывающий возможности источников энергии космических кораблей.
	/// </summary>
	public abstract class Energy
	{
		/// <summary>
		/// Количество оставшейся энергии источнике.
		/// </summary>
		public int Volume { get; protected set; }

		/// <summary>
		/// Использовать энергию из источника.
		/// </summary>
		/// <param name="volume">Количество потребляемой энергии.</param>
		/// <returns>Оставшаяся энергия в источнике.</returns>
		public abstract int Using(int volume);
	}

	/// <summary>
	/// Источник энергии на основе солнечной радиации.
	/// </summary>
	public class SunEnergy : Energy
	{
		/// <inheritdoc />
		public override int Using(int volume)
		{
			// Пусть это будет идеальный бесконечный источник энергии.
			return Volume;
		}

		/// <summary>
		/// Создать экземпляр источника энергии на основе солнечной радиации.
		/// </summary>
		public SunEnergy()
		{
			Volume = 100;
		}
	}

	/// <summary>
	/// Источник энергии на основе плазмы.
	/// </summary>
	public class PlasmEnergy : Energy
	{
		/// <inheritdoc />
		public override int Using(int volume)
		{
			// Обычный расход энергии при использовании.
			if(Volume >= volume)
			{
				Volume -= volume;
				return Volume;
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		/// Создать экземпляр источника энергии на основе плазмы.
		/// </summary>
		public PlasmEnergy()
		{
			Volume = 100;
		}
	}
	#endregion

	#region Gun
	/// <summary>
	/// Базовый абстрактный класс, описывающий возможности оружия космических кораблей.
	/// </summary>
	public abstract class Gun
	{
		/// <summary>
		/// Максимально возможная дистанция между комическими кораблями для выстрела из оружия.
		/// </summary>
		public int Distance { get; protected set; }

		/// <summary>
		/// Выстрелить из оружия.
		/// </summary>
		/// <returns>Количество нанесенного урона.</returns>
		public abstract int Shoot();
	}

	/// <summary>
	/// Лазерная пушка.
	/// </summary>
	public class LaserGun : Gun
	{
		/// <inheritdoc />
		public override int Shoot()
		{
			// Стреляет не сильно, но стабильно
			return 30;
		}

		/// <summary>
		/// Создать экземпляр лазерной пушки.
		/// </summary>
		public LaserGun()
		{
			Distance = 100;
		}
	}

	/// <summary>
	/// Фотонная пушка.
	/// </summary>
	public class PhotonGun : Gun
	{
		/// <summary>
		/// Генератор случайных чисел.
		/// </summary>
		private Random _random = new Random();

		/// <summary>
		/// Минимально возможный урон.
		/// </summary>
		private readonly int _minDmg = 10;

		/// <summary>
		/// Максимально возможный урон.
		/// </summary>
		private readonly int _maxDmg = 80;

		/// <summary>
		/// Вероятность осечки.
		/// </summary>
		private readonly int _missСhance = 10;

		/// <inheritdoc />
		public override int Shoot()
		{
			// Не стабильный урон, может сильно выстрелить, может слабо, а может вовсе не выстрелить, но стреляет на большей дистанции.
			var miss = _random.Next(0, 100);
			if(miss < _missСhance)
			{
				return 0;
			}

			var dmg = _random.Next(_minDmg, _maxDmg);
			return dmg;
		}

		/// <summary>
		/// Создать экземпляр фотонной пушки.
		/// </summary>
		public PhotonGun()
		{
			Distance = 300;
		}
	}
	#endregion

	#region Engine
	/// <summary>
	/// Базовый абстрактный класс, описывающий возможности двигателей космических кораблей.
	/// </summary>
	public abstract class Engine
	{
		/// <summary>
		/// Количество используемой энергии.
		/// </summary>
		public int UsingEnergy { get; protected set; } = 1;

		/// <summary>
		/// Выполнить полет.
		/// </summary>
		/// <param name="energy">Используемый для полета источник энергии.</param>
		/// <returns>Расстояние, на которое выполнился перелет.</returns>
		public virtual int Move(Energy energy)
		{
			energy.Using(UsingEnergy);
			return 1;
		}
	}

	/// <summary>
	/// Импульсный двигатель.
	/// </summary>
	public class PulseEngine : Engine
	{
		/// <summary>
		/// Множитель скорости полета.
		/// </summary>
		private readonly int _speedFactor = 5;

		/// <inheritdoc />
		public override int Move(Energy energy)
		{
			// Стандартный режим полета. Не очень быстро, но стабильно.
			var baseSpeed = base.Move(energy);
			return baseSpeed * _speedFactor;
		}

		/// <summary>
		/// Создать экземпляр импульсного двигателя.
		/// </summary>
		public PulseEngine() { }
	}

	/// <summary>
	/// Фотонный двигатель.
	/// </summary>
	public class PhotonEngine : Engine
	{
		/// <summary>
		/// Генератор случайных чисел.
		/// </summary>
		private Random _random = new Random();

		/// <summary>
		/// Максимальный множитель скорости.
		/// </summary>
		private readonly int _maxFactor = 10;
		public override int Move(Energy energy)
		{
			// Очень нестабильный, но потенциально быстрый двигатель.

			// Потребляет случайное количество энергии.
			int factorEnergy = _random.Next(0, _maxFactor);
			energy.Using(UsingEnergy * factorEnergy);

			// Движется со случайной скоростью или вообще останавливается.
			int factorSpeed = _random.Next(0, _maxFactor);
			return UsingEnergy * factorSpeed;
		}

		/// <summary>
		/// Создать экземпляр фотонного двигателя.
		/// </summary>
		public PhotonEngine()
		{
			UsingEnergy = 3;
		}
	}
	#endregion

	#region Spaceship types
	/// <summary>
	/// Пиратский космический корабль.
	/// </summary>
	public class PiratSpaceshipFactory : SpaceshipFactory
	{
		/// <summary>
		/// Создать экземпляр пиратского космического корабля.
		/// </summary>
		public PiratSpaceshipFactory()
		{
			Health = 200;
			Type = "Пиратский корабль";
		}

		/// <summary>
		/// Создаем источник энергии пиратского комического корабля.
		/// </summary>
		/// <returns>Плазменный источник энергии.</returns>
		public override Energy CreateEnergy()
		{
			return new PlasmEnergy();
		}

		/// <summary>
		/// Создаем оружие пиратского комического корабля.
		/// </summary>
		/// <returns>Фононная пушка.</returns>
		public override Gun CreateGun()
		{
			return new PhotonGun();
		}

		/// <summary>
		/// Создаем двигатель пиратского космического корабля.
		/// </summary>
		/// <returns>Фотонный двигатель.</returns>
		public override Engine CreateEngine()
		{
			return new PhotonEngine();
		}
	}

	/// <summary>
	/// Военный космический корабль.
	/// </summary>
	public class WarSpaceshipFactory : SpaceshipFactory
	{
		/// <summary>
		/// Создать экземпляр военного космического корабля.
		/// </summary>
		public WarSpaceshipFactory()
		{
			Health = 500;
			Type = "Военное судно";
		}

		/// <summary>
		/// Создать источник энергии военного космического корабля.
		/// </summary>
		/// <returns>Плазменный двигатель.</returns>
		public override Energy CreateEnergy()
		{
			return new PlasmEnergy();
		}

		/// <summary>
		/// Создать оружие военного космического корабля.
		/// </summary>
		/// <returns>Лазерная пушка.</returns>
		public override Gun CreateGun()
		{
			return new LaserGun();
		}

		/// <summary>
		/// Создать двигатель военного космического корабля.
		/// </summary>
		/// <returns>Импульсный двигатель.</returns>
		public override Engine CreateEngine()
		{
			return new PulseEngine();
		}
	}
	#endregion

	/// <summary>
	/// Космический корабль.
	/// </summary>
	public class Spaceship
	{
		/// <summary>
		/// Название космического корабля.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Тип космического корабля.
		/// </summary>
		public string Type { get; private set; }

		/// <summary>
		/// Запас здоровья космического корабля.
		/// </summary>
		public int Health { get; private set; }

		/// <summary>
		/// Источник энергии космического корабля.
		/// </summary>
		private Energy _energy;

		/// <summary>
		/// Оружие космического корабля.
		/// </summary>
		private Gun _gun;

		/// <summary>
		/// Двигатель космического корабля.
		/// </summary>
		private Engine _engine;

		/// <summary>
		/// Создать экземпляр космического корабля.
		/// </summary>
		/// <param name="name">Название корабля.</param>
		/// <param name="factory">Фабрика, создающая космический корабль.</param>
		public Spaceship(string name, SpaceshipFactory factory)
		{
			Name = name;
			Type = factory.Type;
			Health = factory.Health;
			_energy = factory.CreateEnergy();
			_gun = factory.CreateGun();
			_engine = factory.CreateEngine();
		}

		/// <summary>
		/// Выстрелить.
		/// </summary>
		/// <returns>Нанесенный урон.</returns>
		public int Shoot()
		{
			return _gun.Shoot();
		}

		/// <summary>
		/// Лететь.
		/// </summary>
		/// <returns>Преодоленное расстояние.</returns>
		public int Move()
		{
			return _engine.Move(_energy);
		}

		/// <summary>
		/// Получить урон.
		/// </summary>
		/// <param name="damage">Величина урона.</param>
		public void TakeDamage(int damage)
		{
			Health -= damage;
		}

		/// <summary>
		/// Приведение объекта к строке.
		/// </summary>
		/// <returns>Полное название космического корабля.</returns>
		public override string ToString()
		{
			return $"{Type} \"{Name}\"";
		}
	}

	/// <summary>
	/// Класс-контроллер отвечающий за проведение боев между космическими кораблями.
	/// </summary>
	public class Battle
	{
		/// <summary>
		/// Первый космический корабль.
		/// </summary>
		private Spaceship _ship1;

		/// <summary>
		/// Второй космический корабль.
		/// </summary>
		private Spaceship _ship2;

		/// <summary>
		/// Создать экземпляр битвы.
		/// </summary>
		/// <param name="ship1">Первый космический корабль.</param>
		/// <param name="ship2">Второй космический корабль.</param>
		public Battle(Spaceship ship1, Spaceship ship2)
		{
			_ship1 = ship1;
			_ship2 = ship2;
		}

		/// <summary>
		/// Бой между космическими кораблями.
		/// </summary>
		/// <returns>Победивший в бою корабль. null - если оба корабля погибли.</returns>
		public Spaceship Fight()
		{
			// Сражаемся насмерть, пока у одного из космических кораблей не закончится здоровье.
			while(_ship1.Health > 0 && _ship2.Health > 0)
			{
				_ship2.TakeDamage(_ship1.Shoot());
				_ship1.TakeDamage(_ship2.Shoot());
			}

			// Выявляем победителя 
			if(_ship1.Health > 0)
			{
				return _ship1;
			}

			if(_ship2.Health > 0)
			{
				return _ship2;
			}

			return null;
		}

		/// <summary>
		/// Гонка между космическими кораблями.
		/// </summary>
		/// <returns>Победивший в гонке космический корабль. null - если оба пролетели одинаковое расстояние.</returns>
		public Spaceship Race()
		{
			// Переменные для подсчета дистанции пройденной кораблями.
			int length1 = 0;
			int length2 = 0;

			// Оба корабля летят на протяжении 100 ходов.
			for(int i = 0; i < 100; i++)
			{
				length1 += _ship1.Move();
				length2 += _ship2.Move();
			}

			// Выявляем победителя.
			if(length1 > length2)
			{
				return _ship1;
			}

			if(length2 > length1)
			{
				return _ship2;
			}

			return null;
		}
	}
}
