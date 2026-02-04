using Godot;
using System;
using System.Collections.Generic;

public partial class StarfieldEffect : Control
{
	// -------------------------------
	// Struct pour chaque étoile
	// -------------------------------
	private struct Star
	{
		public Vector2 pos;
		public float radius;
		public float alphaPhase;
		public float alphaSpeed;
	}

	// -------------------------------
	// Champs de la classe
	// -------------------------------
	private List<Star> _stars = new List<Star>();
	private RandomNumberGenerator rng = new RandomNumberGenerator();

	[Export] public int StarCount = 50;

	// -------------------------------
	// _Ready : initialisation
	// -------------------------------
	public override void _Ready()
	{
		rng.Randomize(); // aléatoire différent à chaque run

		// Générer toutes les étoiles d’un coup
		for (int i = 0; i < StarCount; i++)
		{
			CreateStar();
		}
	}

	// -------------------------------
	// Crée une étoile avec des valeurs aléatoires
	// -------------------------------
	public void CreateStar()
	{
		Star star = new Star
		{
			pos = new Vector2(
				rng.RandfRange(0, Size.X),
				rng.RandfRange(0, Size.Y)
			),
			radius = rng.RandfRange(0.3f, 2.0f),
			alphaPhase = rng.RandfRange(0, Mathf.Tau),
			alphaSpeed = rng.RandfRange(0.8f, 1.80f)
		};

		_stars.Add(star);
	}

	// -------------------------------
	// _Process : mettre à jour alphaPhase pour le scintillement
	// -------------------------------
	public override void _Process(double delta)
	{
		for (int i = 0; i < _stars.Count; i++)
		{
			Star s = _stars[i];

			// faire avancer la phase pour scintillement
			s.alphaPhase += s.alphaSpeed * (float)delta;

			// garder la phase dans 0 → 2π
			if (s.alphaPhase > Mathf.Tau)
				s.alphaPhase -= Mathf.Tau;

			_stars[i] = s; // réassigner le struct
		}

		QueueRedraw(); // redessiner l’écran
	}

	// -------------------------------
	// _Draw : dessiner toutes les étoiles avec scintillement
	// -------------------------------
	public override void _Draw()
	{
		foreach (Star s in _stars)
		{
			// alpha = 0.3 + sin(phase) * 0.3 → oscille entre 0 et 0.6
			float alpha = 0.3f + Mathf.Sin(s.alphaPhase) * 0.3f;
			alpha = Mathf.Clamp(alpha, 0f, 1f);

			Color color = new Color(1, 1, 1, alpha);
			DrawCircle(s.pos, s.radius, color);
		}
	}

	// -------------------------------
	// Regénérer les étoiles si la taille change
	// -------------------------------
	public override void _Notification(int what)
	{
		if (what == NotificationResized)
		{
			_stars.Clear();
			for (int i = 0; i < StarCount; i++)
				CreateStar();
		}
	}
}
