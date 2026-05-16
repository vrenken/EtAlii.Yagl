use rusqlite::{params, Connection, Result};
use serde::{Deserialize, Serialize};

#[derive(Serialize, Deserialize, Debug, Clone)]
pub struct Entity {
    pub id: i32,
    pub name: String,
    pub description: Option<String>,
}

pub fn init_db() -> Result<Connection> {
    let conn = Connection::open_in_memory()?;

    conn.execute(
        "CREATE TABLE entities (
            id          INTEGER PRIMARY KEY,
            name        TEXT NOT NULL,
            description TEXT
        )",
        [],
    )?;

    seed_data(&conn)?;

    Ok(conn)
}

fn seed_data(conn: &Connection) -> Result<()> {
    conn.execute(
        "INSERT INTO entities (name, description) VALUES (?1, ?2)",
        params!["ProjectX", "A secret project"],
    )?;
    conn.execute(
        "INSERT INTO entities (name, description) VALUES (?1, ?2)",
        params!["UserAdmin", "Administrator user"],
    )?;
    conn.execute(
        "INSERT INTO entities (name, description) VALUES (?1, ?2)",
        params!["Requirement1", "System must be fast"],
    )?;
    Ok(())
}

pub fn get_entities(conn: &Connection) -> Result<Vec<Entity>> {
    let mut stmt = conn.prepare("SELECT id, name, description FROM entities")?;
    let entity_iter = stmt.query_map([], |row| {
        Ok(Entity {
            id: row.get(0)?,
            name: row.get(1)?,
            description: row.get(2)?,
        })
    })?;

    let mut entities = Vec::new();
    for entity in entity_iter {
        entities.push(entity?);
    }

    Ok(entities)
}
