namespace stub

open System.Collections.Generic
open System.Linq

module Events =
    let normalEvent =
        [| 0x17uy; 0x20uy; 0x00uy; 0x00uy; 0x78uy; 0x37uy; 0x2auy; 0x18uy;
           0x30uy; 0x26uy; 0x01uy; 0x00uy; 0x13uy; 0x01uy; 0x04uy; 0x02uy;
           0xa0uy; 0x7auy; 0x99uy; 0x00uy; 0x20uy; 0x24uy; 0x11uy; 0x10uy;
           0x12uy; 0x34uy; 0x56uy; 0x06uy; 0x01uy; 0x00uy; 0x01uy; 0x01uy;
           0x01uy; 0x01uy; 0x00uy; 0x01uy; 0x1buy; 0x14uy; 0x37uy; 0x53uy;
           0xe3uy; 0x55uy; 0x00uy; 0x00uy; 0x21uy; 0x00uy; 0x00uy; 0x00uy;
           0x9auy; 0x07uy; 0x09uy; 0x24uy; 0x11uy; 0x13uy; 0x00uy; 0x00uy;
           0x93uy; 0x26uy; 0x04uy; 0x88uy; 0x08uy; 0x92uy; 0x00uy; 0x00uy; |]

    let eventWithoutEvent =
        [| 0x17uy; 0x20uy; 0x00uy; 0x00uy; 0x79uy; 0x37uy; 0x2auy; 0x18uy;
           0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy;
           0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy;
           0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x01uy; 0x00uy; 0x01uy; 0x01uy;
           0x01uy; 0x01uy; 0x00uy; 0x01uy; 0x1buy; 0x14uy; 0x37uy; 0x53uy;
           0xe3uy; 0x55uy; 0x00uy; 0x00uy; 0x21uy; 0x00uy; 0x00uy; 0x00uy;
           0x9auy; 0x07uy; 0x09uy; 0x24uy; 0x11uy; 0x13uy; 0x00uy; 0x00uy;
           0x93uy; 0x26uy; 0x04uy; 0x88uy; 0x08uy; 0x92uy; 0x00uy; 0x00uy; |]

    let errorEvent =
        [| 0x17uy; 0xffuy; 0x00uy; 0x00uy; 0x78uy; 0x37uy; 0x2auy; 0x18uy;
           0xc8uy; 0x00uy; 0x00uy; 0x00uy; 0x01uy; 0x00uy; 0x01uy; 0x01uy;
           0x50uy; 0xffuy; 0x0fuy; 0x00uy; 0x20uy; 0x24uy; 0x12uy; 0x13uy;
           0x10uy; 0x23uy; 0x27uy; 0x12uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy;
           0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x10uy; 0x23uy; 0x27uy;
           0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy;
           0x00uy; 0x00uy; 0x00uy; 0x24uy; 0x12uy; 0x13uy; 0x00uy; 0x00uy;
           0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy |]
