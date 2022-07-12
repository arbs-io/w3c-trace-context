# w3c-trace-context

The [W3C Trace Context](https://www.w3.org/TR/trace-context/) specification defines HTTP headers and formats to propagate the distributed tracing context information. In addition, it defines two fields that should be propagated in the HTTP request's header throughout the trace flow. Take a look below at the standard definition of each field:

- **traceparent**: identifier responsible for describing the incoming request position in its trace graph. It represents a standard format of the incoming request in a tracing system, understood by all vendors.

- **tracestate**: extends **traceparent** with vendor-specific data represented by a set of name/value pairs. Storing information in tracestate is optional.
  
The traceparent field uses the Augmented Backus-Naur Form (ABNF) notation of [RFC5234](https://www.w3.org/TR/trace-context/#bib-rfc5234) and is composed of 4 sub-fields:

- **version** (8-bit): trace context version that the system has adopted. The current is 00.
  
    **version-format**   = trace-id "-" parent-id "-" trace-flags
  - trace-id         = 32HEXDIGLC  ; 16 bytes array identifier. All zeroes forbidden
  - parent-id        = 16HEXDIGLC  ; 8 bytes array identifier. All zeroes forbidden
  - trace-flags      = 2HEXDIGLC   ; 8 bit flags. Currently, only one bit is used.

  Format: 
<span style="color:#2986cc"> version </span> - 
<span style="color:#f1c232"> trace-id </span> - 
<span style="color:#8fce00"> parent-id </span> - 
<span style="color:#6fa8dc"> traceflag </span>

  Example: 
<span style="color:#2986cc"> 00 </span> -
<span style="color:#f1c232"> 3544577c3d9674df0c9375ce5c44a719 </span> - 
<span style="color:#8fce00"> 4340d417f91232fa </span> - 
<span style="color:#6fa8dc"> 00 </span>

</br>

- **trace-id** (16-byte array): This is the ID of the whole trace forest and is used to uniquely identify a distributed trace through a system. It is represented as a 16-byte array, for example, 3544577c3d9674df0c9375ce5c44a719. All bytes as zero (00000000000000000000000000000000) is considered an invalid value.

    If the trace-id value is invalid (for example if it contains non-allowed characters or all zeros), vendors MUST ignore the traceparent.

- **parent-id** (8-byte array): This is the ID of this request as known by the caller (in some tracing systems, this is known as the span-id, where a span is the execution of a client request). It is represented as an 8-byte array, for example, 4340d417f91232fa. All bytes as zero (0000000000000000) is considered an invalid value.

    Vendors MUST ignore the traceparent when the parent-id is invalid (for example, if it contains non-lowercase hex characters).

- **trace-flags** (8-bit): Controls tracing flags such as sampling, trace level, etc. These flags are recommendations given by the caller rather than strict rules to follow for three reasons: "Trust and abuse", "Bug in the caller", "Different load between caller service and callee service might force callee to downsample"

Therefore, applying the trace context concept in an application like the Figure 1 will result in the diagram below:

![Tux, the Linux mascot](/images/traceparent-demo.png)