using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public abstract class GameObject
{
    private Texture2D sprite;
    protected Texture2D[] sprites;
    protected Vector2 position;
    protected float rotation;
    protected float fps;
    private float timeElapsed;
    private int currentIndex;
    protected Vector2 velocity;
    protected float speed;
    protected Rectangle collisionBox;
    protected float scale = 1f;
    private bool collisionEnabled = true;
    protected Vector2 size;
    protected SpriteEffects spriteEffects = SpriteEffects.None;

    public Texture2D Sprite { get => sprite; set => sprite = value; }
    public Vector2 Position { get => position; set => position = value; }
    public Rectangle CollisionBox { get => collisionBox; }
    protected int CurrentIndex { get => currentIndex; set => currentIndex = value; }
    public Vector2 Size { get => size; set => size = value; } //TODO: It is currently the subclass' responsibility it to calculate Size, in the future it should be this class that does it, somehow..
    public bool CollisionEnabled { get => collisionEnabled; set => collisionEnabled = value; }

    protected bool shouldBeRemoved;
    public bool ShouldBeRemoved { get { return shouldBeRemoved; } }

    public abstract void LoadContent(ContentManager contentManager);

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Sprite, Position, null, Color.White, rotation, new Vector2(Sprite.Width / 2, Sprite.Height / 2), scale, spriteEffects, 0);

        if (collisionEnabled)
        {
            collisionBox = new Rectangle((int)position.X - (Sprite.Width / 2 * (int)scale), (int)position.Y - (Sprite.Height / 2 * (int)scale), sprite.Width * (int)scale, sprite.Height * (int)scale);
        }
    }

    public abstract void Update(GameTime gameTime);

    protected void Animate(GameTime gameTime)
    {
        //Undgår fejl, hvis enemy ikke spawner
        if (sprites == null || sprites.Length == 0) return;

        timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

        currentIndex = (int)(timeElapsed * fps);

        sprite = sprites[currentIndex];

        if (currentIndex >= sprites.Length - 1)
        {
            timeElapsed = 0;
            currentIndex = 0;
        }
    }

    protected void ChangeAnimationSprites(Texture2D[] sprites)
    {
        if (this.sprites == sprites) return;

        currentIndex = 0;
        timeElapsed = 0; //Reset timeElapsed because it could cause currentIndex to be bigger than the length of sprites
        this.sprites = sprites;
    }

    protected void Move(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        position += velocity * speed * deltaTime;
    }

    public bool IsColliding(GameObject other)
    {
        if (this == other)
            return true;

        return CollisionBox.Intersects(other.CollisionBox);
    }

    public abstract void OnCollision(GameObject other);

    public void CheckCollision(GameObject other)
    {
        if (!collisionEnabled) return;

        if (CollisionBox.Intersects(other.CollisionBox))
        {
            OnCollision(other);
        }
    }

}