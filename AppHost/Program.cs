var builder = DistributedApplication.CreateBuilder(args);

var localStack = builder.AddResource(new ContainerResource("localstack"))
    .WithImage("localstack/localstack", "3.5")
    .WithImageRegistry("docker.io")
    .WithHttpEndpoint(targetPort: 4566)
    .WithEnvironment("LOCALSTACK_DEBUG", "1");

/*
 * Mounting via bind mount
 * 
 * This get the full path to the source, using `builder.ApplicationBuilder.AppHostDirectory`
 * This causes the path to be transformed to `C:\var\run\docker.sock
 * This folder is created, and mounted, but does not allow access to the socket.
 */
// uncomment the following line to mount the docker socket via a bind mount.
//localStack.WithBindMount("/var/run/docker.sock", "/var/run/docker.sock", isReadOnly: true);

/*
 * Mounting via container mount annotation
 * 
 * This get around the path issue above, but specifying the container mount annotation directly.
 * The path still gets created, and as it forces a windows path, creates the folder at `C:\var\run\docker.sock`
 * The path to be mounted appears to be correct as `/var/run/docker.sock` is available in the container.
 */
// uncomment the following line to mount the docker socket via a container mount annotation.
//localStack.WithAnnotation(new ContainerMountAnnotation("/var/run/docker.sock", "/var/run/docker.sock", ContainerMountType.BindMount, isReadOnly: true));

/*
 * Mounting via direct arguments
 * 
 * This fails as something seems to be happening with the arguments.
 * This fails in `dcpctrl` as the command seems to be adjusted to `/var/run/docker.sock: " /var/run/docker.sock"` which is invalid
 */
// uncomment the following line to mount the docker socket via container runtime args.
//localStack.WithContainerRuntimeArgs("-v /var/run/docker.sock:/var/run/docker.sock");

builder.Build().Run();
