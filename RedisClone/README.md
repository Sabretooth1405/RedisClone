# RedisClone

A Redis-compatible server built from scratch in C#. Implements the RESP (Redis Serialization Protocol) over raw TCP sockets, with a concurrent client handler, key expiry, and a tested executor pipeline.

## Features

- RESP protocol parser and serializer (Simple Strings, Errors, Integers, Bulk Strings, Arrays)
- TCP server on port 6380 with concurrent client support via threading
- Command pipelining support
- Key-value store backed by `ConcurrentDictionary`
- Key expiry with `EX`, `PX`, `EXAT`, `PXAT` options (lazy expiry)
- Compatible with `redis-cli` and `redis-benchmark`

## Supported Commands

| Command | Example |
|---|---|
| `PING` | `PING` → `PONG` |
| `ECHO` | `ECHO hello` → `hello` |
| `SET` | `SET key value [EX seconds]` |
| `GET` | `GET key` |

## Getting Started

### Prerequisites
- .NET 8 SDK
- `redis-cli` (optional, for manual testing)

### Run

```bash
cd RedisClone
dotnet run
```

Server starts on port 6380.

### Test with redis-cli

```bash
redis-cli -p 6380 PING
redis-cli -p 6380 SET name Ankit
redis-cli -p 6380 GET name
redis-cli -p 6380 SET temp value EX 5
```

### Benchmark

```bash
redis-benchmark -p 6380 -t SET,GET -q
```

Typical throughput on a MacBook: ~16k SET/sec, ~18k GET/sec.

### Run Tests

```bash
cd RedisClone.Tests
dotnet test
```

## Project Structure

```
RedisClone/
├── Resp/               # RESP types and parser
│   ├── Parser.cs
│   ├── RespMessage.cs
│   ├── RespSimpleString.cs
│   ├── RespBulkString.cs
│   ├── RespError.cs
│   ├── RespInteger.cs
│   └── RespArray.cs
├── Executors/          # Command handlers
│   ├── Executable.cs
│   ├── PingExecutor.cs
│   ├── EchoExecutor.cs
│   ├── SetExecutor.cs
│   └── GetExecutor.cs
├── RedisStore.cs       # Singleton key-value store
├── RedisObj.cs         # Value wrapper with expiry
├── Handler.cs          # Command dispatch
└── Program.cs          # TCP server entry point

RedisCloneTest/
└── RespParserTests.cs  # RESP parsing + executor tests
```

## Architecture Notes

- One thread per client connection
- Lazy expiry — keys are checked and deleted on access, not on a background timer
- RESP pipelining handled by splitting raw input on `*` command boundaries
