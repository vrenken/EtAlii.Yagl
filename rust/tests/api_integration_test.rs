use tokio::time::{sleep, Duration};
use reqwest::StatusCode;
use std::process::{Command, Child, Stdio};
use serde::{Deserialize, Serialize};

#[derive(Serialize, Deserialize, Debug)]
struct Entity {
    id: i32,
    name: String,
    description: Option<String>,
}

struct ChildGuard(Child);

impl Drop for ChildGuard {
    fn drop(&mut self) {
        let _ = self.0.kill();
    }
}

#[tokio::test]
async fn test_query_entities() {
    // Arrange: Start the server
    let child = Command::new("cargo")
        .arg("run")
        .stdout(Stdio::piped())
        .spawn()
        .expect("Failed to start server");
    
    let _guard = ChildGuard(child);

    // Wait for the server to start
    let mut attempts = 0;
    let client = reqwest::Client::new();
    loop {
        match client.get("http://127.0.0.1:3000/entities").send().await {
            Ok(resp) if resp.status() == StatusCode::OK => break,
            _ => {
                if attempts > 30 {
                    panic!("Server failed to start in time");
                }
                sleep(Duration::from_secs(1)).await;
                attempts += 1;
            }
        }
    }

    // Act: Query the data
    let response = client.get("http://127.0.0.1:3000/entities")
        .send()
        .await
        .expect("Failed to send request");

    // Assert: Verify the data
    assert_eq!(response.status(), StatusCode::OK);
    let entities: Vec<Entity> = response.json().await.expect("Failed to parse JSON");
    
    assert!(entities.len() >= 3);
    assert_eq!(entities[0].name, "ProjectX");
    assert_eq!(entities[1].name, "UserAdmin");
    assert_eq!(entities[2].name, "Requirement1");
}
