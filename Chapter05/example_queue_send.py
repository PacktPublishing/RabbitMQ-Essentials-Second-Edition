import pika

import json

credentials = pika.PlainCredentials("cc-dev", "taxi123")
parameters = pika.ConnectionParameters(
    host="127.0.0.1",
    port=5672,
    virtual_host="cc-dev-ws",
    credentials=credentials)

conn = pika.BlockingConnection(parameters)
assert conn.is_open

try:
    ch = conn.channel()
    assert ch.is_open
    headers = {"version": "0.1b", "system": "taxi"}
    properties = pika.BasicProperties(content_type="application/json", headers=headers)
    message = {"latitude": 0.0, "longitude": -1.0}
    message = json.dumps(message)
    ch.basic_publish(
        exchange="taxi_header_exchange",
        body=message,
        properties=properties, routing_key="")
finally:
    conn.close()
