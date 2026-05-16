mod db;

use pest::Parser;
use pest_derive::Parser;
use axum::{routing::get, Json, Router, extract::State};
use std::sync::{Arc, Mutex};
use rusqlite::Connection;

#[derive(Parser)]
#[grammar = "yagl.pest"]
pub struct YAGLParser;

#[tokio::main]
async fn main() {
    let conn = db::init_db().expect("Failed to initialize database");
    let shared_conn = Arc::new(Mutex::new(conn));

    let app = Router::new()
        .route("/entities", get(list_entities))
        .with_state(shared_conn);

    let listener = tokio::net::TcpListener::bind("127.0.0.1:3000").await.unwrap();
    println!("listening on {}", listener.local_addr().unwrap());
    axum::serve(listener, app).await.unwrap();
}

async fn list_entities(
    State(conn): State<Arc<Mutex<Connection>>>,
) -> Json<Vec<db::Entity>> {
    let conn = conn.lock().unwrap();
    let entities = db::get_entities(&conn).expect("Failed to get entities");
    Json(entities)
}

pub fn add(a: i32, b: i32) -> i32 {
    a + b
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_add() {
        // Arrange
        let a = 2;
        let b = 2;
        let expected = 4;

        // Act
        let result = add(a, b);

        // Assert
        assert_eq!(result, expected);
    }

    #[test]
    fn test_parse_sample1() {
        // Arrange
        let content = std::fs::read_to_string("tests/yagl_files/sample1.yagl").expect("Could not read sample1.yagl");

        // Act
        let result = YAGLParser::parse(Rule::program, &content);

        // Assert
        assert!(result.is_ok(), "Parsing sample1.yagl failed: {:?}", result.err());
    }

    #[test]
    fn test_parse_sample2() {
        // Arrange
        let content = std::fs::read_to_string("tests/yagl_files/sample2.yagl").expect("Could not read sample2.yagl");

        // Act
        let result = YAGLParser::parse(Rule::program, &content);

        // Assert
        assert!(result.is_ok(), "Parsing sample2.yagl failed: {:?}", result.err());
    }

    #[test]
    fn test_parse_sample3() {
        // Arrange
        let content = std::fs::read_to_string("tests/yagl_files/sample3.yagl").expect("Could not read sample3.yagl");

        // Act
        let result = YAGLParser::parse(Rule::program, &content);

        // Assert
        assert!(result.is_ok(), "Parsing sample3.yagl failed: {:?}", result.err());
    }

    #[test]
    fn test_parse_wildcards() {
        // Arrange
        let content = std::fs::read_to_string("tests/yagl_files/wildcards.yagl").expect("Could not read wildcards.yagl");

        // Act
        let result = YAGLParser::parse(Rule::program, &content);

        // Assert
        assert!(result.is_ok(), "Parsing wildcards.yagl failed: {:?}", result.err());
    }
}
